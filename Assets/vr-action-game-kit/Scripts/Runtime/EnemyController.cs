using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("敵をふっとばす高さ")] [SerializeField] private float enemyAddForcePowerY;
    [Header("敵をふっとばす奥行き")] [SerializeField] private float enemyAddForcePowerZ;
    [SerializeField] private Rigidbody enemyRigidbody;

    // 敵が倒された際の処理を行います
    public void Beat()
    {
        //敵のヒットストップ処理を行います
        
        //敵がやられた際の音を鳴らします
        
        //敵を吹っ飛ばします
        enemyRigidbody.AddForce(0, enemyAddForcePowerY, enemyAddForcePowerZ);
    }
}
