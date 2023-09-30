//
//IKCtrlRightHand.cs
//
//Sample script for IK Control of Unity-Chan's right hand.
//
//2014/06/20 N.Kobayashi
//

using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityChan
{
	[RequireComponent(typeof(Animator))]
	public class IKCtrlRightHand : MonoBehaviour
	{

		private Animator anim;
		public bool isAttacking = false;
		[SerializeField] private Transform targetObj = null;
		[SerializeField] private Transform targetObjRotation = null;
		[SerializeField] private GameObject weapon;
		[SerializeField] private GameObject weaponGripHand;
		[SerializeField] private  bool isIkActive = false;
		[SerializeField] private  float mixWeight = 1.0f;
		[SerializeField] private  float attackWaitTime;
		[SerializeField] private  float swingDuration;

		void Awake ()
		{
			anim = GetComponent<Animator> ();
		}

		private void Start()
		{
			//エディタ上でTrailRendererの動作確認をするために有効化しているため、プレイ時に無効化します
			weapon.GetComponentInChildren<TrailRenderer>().enabled = false;
			
			//武器についているVR用のコンポーネントをオフにします
			weapon.GetComponent<XRGrabInteractable>().enabled = false;
			weapon.GetComponent<Rigidbody>().isKinematic = true;
			
			//手の子オブジェクトにすることで手の動きに合わせて動くようにします
			weapon.transform.parent = weaponGripHand.transform;
		}

		void FixedUpdate ()
		{
			//Kobayashi
			if (mixWeight >= 1.0f)
				mixWeight = 1.0f;
			else if (mixWeight <= 0.0f)
				mixWeight = 0.0f;
		}
		
        enum AttackState
        {
            None,
            StartAttacking,
            SwingWeapon,
            WaitAttacking,
        };

        private AttackState attackState = AttackState.None;
        private float swingingTime = 0f;
        private float waitAttackingTime = 0f;

        void OnAnimatorIK(int layerIndex)
        {
            if (isIkActive)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, mixWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, mixWeight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, targetObj.position);

                Quaternion startRotation = targetObj.rotation;
                Quaternion targetRotation = targetObjRotation.rotation;

                switch (attackState)
                {
                    case AttackState.None:
                        if (Input.GetButtonDown("Fire1"))
                        {
                            if (!weapon.GetComponent<WeaponController>().IsAttacking())
                            {
                                // 攻撃開始ステートに移行
                                attackState = AttackState.StartAttacking;
                            }
                        }
                        break;

                    case AttackState.StartAttacking:
                        // 攻撃開始のための初期化
                        weapon.GetComponent<WeaponController>().AttackStart();
                        weapon.GetComponentInChildren<TrailRenderer>().enabled = true;
                        swingingTime = 0f;
                        // 武器を振るステートに移行
                        attackState = AttackState.SwingWeapon;
                        break;

                    case AttackState.SwingWeapon:
                        // swingDurationの時間をかけて振る
                        if (swingingTime < swingDuration)
                        {
                            float t = swingingTime / swingDuration;
                            Quaternion currentRotation = Quaternion.Lerp(startRotation, targetRotation, t);
                            anim.SetIKRotation(AvatarIKGoal.RightHand, currentRotation);

                            swingingTime += Time.deltaTime;
                        }
                        // 振り終わったら次の攻撃待ちステートに移行
                        else
                        {
                            anim.SetIKRotation(AvatarIKGoal.RightHand, targetRotation);
                            waitAttackingTime = 0f;
                            attackState = AttackState.WaitAttacking;
                        }
                        break;

                    case AttackState.WaitAttacking:
                        anim.SetIKRotation(AvatarIKGoal.RightHand, targetRotation);
                        waitAttackingTime += Time.deltaTime;
                        // 攻撃待ちが済んだら攻撃の終了処理をして通常ステートに移行
                        if (waitAttackingTime >= attackWaitTime)
                        {
                            weapon.GetComponentInChildren<TrailRenderer>().enabled = false;
                            weapon.GetComponent<WeaponController>().AttackEnd();

                            attackState = AttackState.None;
                        }
                        break;
                }

                // 通常時の回転値を右手に割り当てればいいステートの場合はここで一括処理
                if (attackState == AttackState.None || attackState == AttackState.StartAttacking)
                {
                    anim.SetIKRotation(AvatarIKGoal.RightHand, targetObj.rotation);
                }
            }
        }
	}
}