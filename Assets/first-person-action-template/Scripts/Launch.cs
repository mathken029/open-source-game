using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] private GameObject watermelonPrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float waitLaunchTime;
    [SerializeField] private float LaunchX;
    [SerializeField] private float LaunchY;
    [SerializeField] private float LaunchZ;

    //武器にぶつかったときの音です
    [SerializeField] private AudioClip seCollisionWatermelon;
    [SerializeField] private AudioClip seBombAttacked;

    //音を再生するためのコンポーネントの情報を格納する変数です
    [SerializeField] private AudioSource audioSource;
    
    //ポイントの加算に使用します
    [SerializeField] private GameObject displayController;
    
    //各オブジェクトのポイントを定義します
    [SerializeField] private int watermelonPoints;
    [SerializeField] private int bombSubtractionPoints;
    


    private bool isLaunching = false;
    private GameObject shell;
    
    private int shellNumber;
    private const int SHELL_WATERMELON = 0;
    private const int SHELL_BOMB = 1;
    
    private const string OBJECTNAME_WATERMELON = "watermelon(Clone)";
    private const string OBJECTNAME_BOMB = "just_a_simple_bomb(Clone)";
    
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

        //ここをこの後乱数にする
        if (shellNumber == SHELL_WATERMELON)
        {
            shellNumber = SHELL_BOMB;
        }
        else if (shellNumber == SHELL_BOMB)
        {
            shellNumber = SHELL_WATERMELON;
        }

        switch (shellNumber)
        {
            case SHELL_WATERMELON:
                shell = Instantiate(watermelonPrefab, transform.position, transform.rotation, transform);
                break;
            
            case SHELL_BOMB:
                shell = Instantiate(bombPrefab, transform.position, transform.rotation, transform);
                break;
        }
        shell.GetComponent<Rigidbody>().AddForce(new Vector3(LaunchX, LaunchY, LaunchZ));
        yield return new WaitForSeconds(waitLaunchTime);
        
        isLaunching = false;
    }

    public void CollisionShell(string objectName)
    {
        switch (objectName)
        {
            case OBJECTNAME_WATERMELON:
                //ポイントを加算して効果音を鳴らします
                displayController.GetComponent<DisplayController>().AddPoints(watermelonPoints);
                audioSource.PlayOneShot(seCollisionWatermelon);
                break;
            
            case OBJECTNAME_BOMB:
                //ポイントを減算して効果音を鳴らします
                displayController.GetComponent<DisplayController>().SubtractionPoints(bombSubtractionPoints);
                audioSource.PlayOneShot(seBombAttacked);
                break;
        }

    }
}
