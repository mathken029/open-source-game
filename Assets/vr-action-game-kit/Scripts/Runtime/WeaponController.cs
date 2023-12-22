using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("武器が再度表示されるまでの時間")]
    [SerializeField] private float weaponEnableTime;

    [SerializeField] private Renderer weaponRenderer01;
    [SerializeField] private Renderer weaponRenderer02;
    [SerializeField] private Renderer weaponRenderer03;
    [SerializeField] private Renderer weaponRenderer04;
    [SerializeField] private Renderer weaponRenderer05;
    [SerializeField] private Renderer weaponRenderer06;
    [SerializeField] private Renderer weaponRenderer07;
    [SerializeField] private Renderer weaponRenderer08;
    [SerializeField] private Renderer weaponRenderer09;
    [SerializeField] private Renderer weaponRenderer10;
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private ParticleSystem weaponParticleSystem;
    [SerializeField] private TrailRenderer weaponTrailRenderer;

    /// <Summary>
    /// どの音を再生するかを設定します
    /// </Summary>
    [SerializeField] private AudioClip seSwordCollision;

    //音を再生するためのコンポーネントの情報を格納する変数です
    [SerializeField] private AudioSource audioSource;

    private bool isAttacking = false;


    public void AttackStart()
    {
        isAttacking = true;
    }
    
    public void AttackEnd()
    {
        isAttacking = false;
    }
    
    public bool IsAttacking()
    {
        return isAttacking;
    }

    /// <Summary>
    /// 武器同士が触れると一定時間消滅するようにします
    /// </Summary>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        
        //当たったのが武器かどうかを判定します
        if (other.gameObject.TryGetComponent<WeaponController>(
                out WeaponController _WeaponControllerIdentification))
        {
            Debug.Log("TryGetComponent");
            //武器同士が衝突したときの音を鳴らします
            audioSource.PlayOneShot(seSwordCollision);
            
            //パーティクルを発します
            weaponParticleSystem.Play();
            
            //武器を消滅させ、一定時間後再度表示します
            StartCoroutine(EnableWeaponCoroutine());
        }
    }
    
    /// <Summary>
    /// 武器を消滅させ、一定時間後再度表示します
    /// </Summary>
    private IEnumerator EnableWeaponCoroutine()
    {
        // 武器の見た目と当たり判定を無効化させます
        weaponRenderer01.enabled = false;
        weaponRenderer02.enabled = false;
        weaponRenderer03.enabled = false;
        weaponRenderer04.enabled = false;
        weaponRenderer05.enabled = false;
        weaponRenderer06.enabled = false;
        weaponRenderer07.enabled = false;
        weaponRenderer08.enabled = false;
        weaponRenderer09.enabled = false;
        weaponRenderer10.enabled = false;
        
        weaponCollider.enabled = false;
        weaponTrailRenderer.enabled = false;
            
        Debug.Log($"EnableWeaponCoroutine() Start");
            
        // 設定された時間待ちます
        yield return new WaitForSecondsRealtime(weaponEnableTime);

        // 武器の見た目と当たり判定を有効化させます
        weaponRenderer01.enabled = true;
        weaponRenderer02.enabled = true;
        weaponRenderer03.enabled = true;
        weaponRenderer04.enabled = true;
        weaponRenderer05.enabled = true;
        weaponRenderer06.enabled = true;
        weaponRenderer07.enabled = true;
        weaponRenderer08.enabled = true;
        weaponRenderer09.enabled = true;
        weaponRenderer10.enabled = true;
        
        weaponCollider.enabled = true;
        weaponTrailRenderer.enabled = true;

        Debug.Log($"EnableWeaponCoroutine() End");

    }
}
