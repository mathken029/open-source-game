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
        
        GameObject watermelon = Instantiate(watermelonPrefab, transform.position, transform.rotation);
        watermelon.GetComponent<Rigidbody>().AddForce(new Vector3(LaunchX, LaunchY, LaunchZ));
        yield return new WaitForSeconds(waitLaunchTime);
        
        isLaunching = false;
    }
    
}
