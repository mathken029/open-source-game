using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChan
{
public class ShellController : MonoBehaviour
{
    //プレイヤーに砲弾がヒットしたときの処理です
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<UnityChanControlScriptWithRgidBody>())
        {
            if (this.transform.GetComponentInParent<Launch>().ShellHit(this.gameObject.name))
            {
                //パーティクルを再生する
                StartCoroutine("HitCoroutine");
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        
        //床に触れたときに消滅します
        if (collision.gameObject.GetComponent<FloorController>())
        {
            //パーティクルを再生する
            StartCoroutine("HitCoroutine");
        }

    }

    //プレイヤーの武器に砲弾がヒットしたときの処理です
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WeaponController>()
                 .TryGetComponent<WeaponController>(out WeaponController _weaponController))
        {
            if (_weaponController.IsAttacking())
            {
                //攻撃中の武器に当たった場合の処理です
                if (this.transform.GetComponentInParent<Launch>().ShellAttacked(this.gameObject.name))
                {
                    //パーティクルを再生する
                    StartCoroutine("CollisionCoroutine");
                }
            }
            else
            {
                //攻撃中ではない武器に当たった場合の処理です
                if (this.transform.GetComponentInParent<Launch>().ShellGuarded(this.gameObject.name))
                {
                    //パーティクルを再生する
                    StartCoroutine("CollisionCoroutine");
                }
            }
        }
    }

    IEnumerator CollisionCoroutine()
    {
        this.gameObject.GetComponent<ParticleSystem>().Play();

        if (gameObject.name == Launch.OBJECTNAME_SPIKEDBALL)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 400));
        }
        else
        {
        
            //パーティクルの位置がずれないように停止します
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        
            //配下の全てのメッシュを無効化して見えなくする
            MeshRenderer[] mrChild = this.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach ( MeshRenderer mr  in mrChild ) {
                mr.enabled = false;
            }        
        
            //2回当たることがあるので当たり判定を無くします
            this.gameObject.GetComponent<Collider>().enabled = false;

            //パーティクルが終わるのを待ってから消滅します
            yield return new WaitForSeconds(0.2f);
        
            Destroy(this.gameObject);
        }
    }
    
    //ヒットしてすぐ消えるので、パーティクルを再生しません
    IEnumerator HitCoroutine()
    {
        //パーティクルの位置がずれないように停止します
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        
        //配下の全てのメッシュを無効化して見えなくする
        MeshRenderer[] mrChild = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach ( MeshRenderer mr  in mrChild ) {
            mr.enabled = false;
        }        
        
        //2回当たることがあるので当たり判定を無くします
        this.gameObject.GetComponent<Collider>().enabled = false;

        //パーティクルが終わるのを待ってから消滅します
        yield return new WaitForSeconds(0.2f);
        
        Destroy(this.gameObject);
    }
}
}

