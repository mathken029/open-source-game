using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーがやられたときの音")] [SerializeField] private AudioClip sePlayerBeated;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private AudioSource audioSource;

    private bool beatFlag = false;

    //プレイヤーがリセットされた際の処理です
    public void Reset()
    {
        beatFlag = false;
    }

    //敵が倒されているかどうかを確認します
    public bool CheckBeated()
    {
        return beatFlag;
    }

    /// <Summary>
    /// 敵の武器が触れたかをチェックします
    /// </Summary>
    private void OnTriggerEnter(Collider other)
    {
        //当たったのが武器かどうかを判定します
        if (other.gameObject.TryGetComponent<EnemyWeaponController>(
                out EnemyWeaponController _enemyWeaponControllerIdentification))
        {
            if (beatFlag == false)
            {
                //プレイヤーがやられたときのサウンドを鳴らします
                audioSource.PlayOneShot(sePlayerBeated);
            }
            //プレイヤーが倒されたフラグを立てます
            beatFlag = true;
        }
    }
}
