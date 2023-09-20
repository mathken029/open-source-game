using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // if not collision to weapon, end method
        if (other.gameObject.GetComponent<WeaponController>() == null)
        {
            return;
        }
        // if collision to weapon, burst
        else if (other.gameObject.GetComponent<WeaponController>()
                 .TryGetComponent<WeaponController>(out WeaponController _weaponController))
        {
            if (_weaponController.IsAttacking())
            {
                this.transform.GetComponentInParent<Launch>().CollisionWatermelon();
            
                //パーティクルを再生する
                StartCoroutine("CollisionCoroutine");
            }
        }

    }

    IEnumerator CollisionCoroutine()
    {
        //パーティクルの位置がずれないように停止します
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        
        //メッシュを無効化して見えなくする
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        
        //2回当たることがあるので当たり判定を無くします
        this.gameObject.GetComponent<Collider>().enabled = false;
        
        this.gameObject.GetComponent<ParticleSystem>().Play();
        
        yield return new WaitForSeconds(0.2f);
        
        Destroy(this.gameObject);
    }
}
