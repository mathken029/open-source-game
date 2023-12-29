using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーが攻撃を受けたときの音")] [SerializeField] private AudioClip sePlayerBeated;
    [Header("プレイヤーが攻撃を受けたときに減るポイント")] [SerializeField] private int damagePoints;
    [SerializeField] private DisplayController displayController;
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
                out EnemyWeaponController enemyWeaponControllerIdentification))
        {
            if (enemyWeaponControllerIdentification.IsAttacking())
            {
                //プレイヤーが攻撃を受けたときのサウンドを鳴らします
                audioSource.PlayOneShot(sePlayerBeated);
            
                //ポイントを減らします
                displayController.SubtractionPoints(damagePoints);
            }
        }
    }
}
