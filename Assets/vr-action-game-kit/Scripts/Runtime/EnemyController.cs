using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("敵が吹っ飛ぶ高さ")] [SerializeField] private float enemyAddForcePowerY;
    [Header("敵が吹っ飛ぶ奥行き")] [SerializeField] private float enemyAddForcePowerZ;
    [Header("敵が吹っ飛ぶ音")] [SerializeField] private AudioClip seEnemyStrike;
    
    [Header("プレイヤーの座標")] [SerializeField] private Transform playerTransform;
    [SerializeField] private NavMeshAgent enemyNavMeshAgent;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Rigidbody enemyRigidbody;
    
    [Header("敵が戦闘モードに入る距離")] [SerializeField] private float battleDistance;

    private bool beatFlag = false;
    
    //敵がリセットされた際の処理です
    public void Reset()
    {
        beatFlag = false;
    }

    //敵が倒されているかどうかを確認します
    public bool CheckBeated()
    {
        return beatFlag;
    }
    
    // 敵が倒された際の処理を行います
    public void Beat()
    {
        beatFlag = true;
        
        //NavmeshAgentが有効だと上に飛ばないので無効化します
        enemyNavMeshAgent.enabled = false;
        
        //敵のヒットストップ処理を行います
        
        //敵がやられた際の音を鳴らします
        audioSource.PlayOneShot(seEnemyStrike);
        
        //敵を吹っ飛ばします
        enemyRigidbody.AddForce(0, enemyAddForcePowerY, enemyAddForcePowerZ);
    }

    private void Update()
    {
        Debug.Log(Vector3.Distance(playerTransform.position, enemyNavMeshAgent.transform.position));
        
        //プレイヤーの位置まで移動します
        if (Vector3.Distance(playerTransform.position, enemyNavMeshAgent.transform.position) >
            battleDistance)
        {
            enemyNavMeshAgent.SetDestination(playerTransform.position);
        }

    }
}
