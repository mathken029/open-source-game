using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] private GameObject watermelonPrefab;
    [SerializeField] private float waitLaunchTime;
    [SerializeField] private float LaunchX;
    [SerializeField] private float LaunchY;
    [SerializeField] private float LaunchZ;

    //武器にぶつかったときの音です
    [SerializeField] private AudioClip seCollisionWatermelon;

    //音を再生するためのコンポーネントの情報を格納する変数です
    [SerializeField] private AudioSource audioSource;
    
    //ポイントの加算に使用します
    [SerializeField] private GameObject displayController;
    
    //各オブジェクトのポイントを定義します
    [SerializeField] private int watermelonPoints;


    private bool isLaunching = false;
    
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
        
        GameObject watermelon = Instantiate(watermelonPrefab, transform.position, transform.rotation, this.transform);
        watermelon.GetComponent<Rigidbody>().AddForce(new Vector3(LaunchX, LaunchY, LaunchZ));
        yield return new WaitForSeconds(waitLaunchTime);
        
        isLaunching = false;
    }

    public void CollisionWatermelon()
    {
        //ポイントの加算処理を書く
        displayController.GetComponent<DisplayController>().AddPoint(watermelonPoints);
    
        audioSource.PlayOneShot(seCollisionWatermelon);
    }
}
