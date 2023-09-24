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
    [SerializeField] private float launchPoint01X;
    [SerializeField] private float launchPoint01Y;
    [SerializeField] private float launchPoint01Z;
    [SerializeField] private float launchPoint02X;
    [SerializeField] private float launchPoint02Y;
    [SerializeField] private float launchPoint02Z;
    [SerializeField] private WeightedList<int> weightedList;
    [SerializeField] private GameObject launchPoint01;
    [SerializeField] private GameObject launchPoint02;

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
        
        //どの砲弾を発射するかを決めます
        int shellNumber = weightedList.RandomElement();
        
        //どちらの砲台から発射するかを決めます
        int canonNumber = LucidRandom.Range(0, 2);

        switch (shellNumber)
        {
            case SHELL_WATERMELON:
                if (canonNumber == 0)
                {
                    shell = Instantiate(watermelonPrefab, launchPoint01.transform.position, launchPoint01.transform.rotation, launchPoint01.transform);
                }
                else
                {
                    shell = Instantiate(watermelonPrefab, launchPoint02.transform.position, launchPoint02.transform.rotation, launchPoint02.transform);
                }
                break;
            
            case SHELL_BOMB:
                if (canonNumber == 0)
                {
                    shell = Instantiate(bombPrefab, launchPoint01.transform.position, launchPoint01.transform.rotation, launchPoint01.transform);
                }
                else
                {
                    shell = Instantiate(bombPrefab, launchPoint02.transform.position, launchPoint02.transform.rotation, launchPoint02.transform);
                }
                break;
            
            case SHELL_SPIKEDBALL:
                if (canonNumber == 0)
                {
                    shell = Instantiate(spikedBallPrefab, launchPoint01.transform.position, launchPoint01.transform.rotation, launchPoint01.transform);
                }
                else
                {
                    shell = Instantiate(spikedBallPrefab, launchPoint02.transform.position, launchPoint02.transform.rotation, launchPoint02.transform);
                }
                break;
            
            case SHELL_NOTHING:
                break;
        }

        if (shellNumber != SHELL_NOTHING)
        {
            audioSource.PlayOneShot(seCanon);
            if (canonNumber == 0)
            {
                shell.GetComponent<Rigidbody>().AddForce(new Vector3(launchPoint01X, launchPoint01Y, launchPoint01Z));
            }
            else
            {
                shell.GetComponent<Rigidbody>().AddForce(new Vector3(launchPoint02X, launchPoint02Y, launchPoint02Z));
            }
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
