using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnnulusGames.LucidTools.RandomKit;

public class Launch : MonoBehaviour
{
    [SerializeField] private GameObject watermelonPrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject spikedBallPrefab;
    [SerializeField] private float waitLaunchTime;
    [SerializeField] private float LaunchX;
    [SerializeField] private float LaunchY;
    [SerializeField] private float LaunchZ;
    [SerializeField] private WeightedList<int> weightedList;

    //武器にぶつかったときの音です
    [SerializeField] private AudioClip seCollisionWatermelon;
    [SerializeField] private AudioClip seBombAttacked;
    [SerializeField] private AudioClip seSpikedBallGuarded;
    [SerializeField] private AudioClip seSpikedBallHit;
    [SerializeField] private AudioClip seCanon;
    
    //音を再生するためのコンポーネントの情報を格納する変数です
    [SerializeField] private AudioSource audioSource;
    
    //ポイントの加算に使用します
    [SerializeField] private GameObject displayController;
    
    //各オブジェクトのポイントを定義します
    [SerializeField] private int watermelonPoints;
    [SerializeField] private int bombSubtractionPoints;
    


    private bool isLaunching = false;
    private GameObject shell;
    
    private const int SHELL_WATERMELON = 0;
    private const int SHELL_BOMB = 1;
    private const int SHELL_SPIKEDBALL = 2;
    private const int SHELL_NOTHING = 3;
    
    private const string OBJECTNAME_WATERMELON = "watermelon(Clone)";
    private const string OBJECTNAME_BOMB = "just_a_simple_bomb(Clone)";
    private const string OBJECTNAME_SPIKEDBALL = "spiked_ball(Clone)";

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLaunching)
        {
            return;
        }
        else
        {
            StartCoroutine("LaunchCoroutine");
        }
    }

    IEnumerator LaunchCoroutine()
    {
        isLaunching = true;
        
        //重み付けした乱数を格納します
        int shellNumber = weightedList.RandomElement();

        switch (shellNumber)
        {
            case SHELL_WATERMELON:
                shell = Instantiate(watermelonPrefab, transform.position, transform.rotation, transform);
                break;
            
            case SHELL_BOMB:
                shell = Instantiate(bombPrefab, transform.position, transform.rotation, transform);
                break;
            
            case SHELL_SPIKEDBALL:
                shell = Instantiate(spikedBallPrefab, transform.position, transform.rotation, transform);
                break;
            
            case SHELL_NOTHING:
                break;
        }

        if (shellNumber != SHELL_NOTHING)
        {
            audioSource.PlayOneShot(seCanon);
            shell.GetComponent<Rigidbody>().AddForce(new Vector3(LaunchX, LaunchY, LaunchZ));
        }
        yield return new WaitForSeconds(waitLaunchTime);
        
        isLaunching = false;
    }

    public bool ShellAttacked(string objectName)
    {
        bool attackFlag = false;
        
        switch (objectName)
        {
            case OBJECTNAME_WATERMELON:
                //ポイントを加算して効果音を鳴らします
                displayController.GetComponent<DisplayController>().AddPoints(watermelonPoints);
                audioSource.PlayOneShot(seCollisionWatermelon);
                attackFlag = true;
                break;
            case OBJECTNAME_BOMB:
                //ポイントを減算して効果音を鳴らします
                displayController.GetComponent<DisplayController>().SubtractionPoints(bombSubtractionPoints);
                audioSource.PlayOneShot(seBombAttacked);
                attackFlag = true;
                break;
            case OBJECTNAME_SPIKEDBALL:
                //防げたときの効果音を鳴らします
                audioSource.PlayOneShot(seSpikedBallGuarded);
                attackFlag = true;
                break;
        }
        
        return attackFlag;
    }
    
    public bool ShellGuarded(string objectName)
    {
        bool guardFlag = false;
        
        switch (objectName)
        {
            case OBJECTNAME_BOMB:
                //ポイントを減算して効果音を鳴らします
                displayController.GetComponent<DisplayController>().SubtractionPoints(bombSubtractionPoints);
                audioSource.PlayOneShot(seBombAttacked);
                guardFlag = true;
                break;
            case OBJECTNAME_SPIKEDBALL:
                //防げたときの効果音を鳴らします
                audioSource.PlayOneShot(seSpikedBallGuarded);
                guardFlag = true;
                break;
        }
        
        return guardFlag;
    }
    
    public bool ShellHit(string objectName)
    {
        bool hitFlag = false;
        
        switch (objectName)
        {
            case OBJECTNAME_SPIKEDBALL:
                //ポイントを減らして、プレイヤーにヒットしたときの効果音を鳴らします
                displayController.GetComponent<DisplayController>().SubtractionPoints(bombSubtractionPoints);
                audioSource.PlayOneShot(seSpikedBallHit);
                hitFlag = true;
                break;
        }
        
        return hitFlag;
    }
}
