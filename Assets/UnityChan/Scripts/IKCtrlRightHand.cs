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
		[SerializeField] private  bool isIkActive = false;
		[SerializeField] private  float mixWeight = 1.0f;
		[SerializeField] private  float attackWaitTime;

		void Awake ()
		{
			anim = GetComponent<Animator> ();
		}

		private void Start()
		{
			//エディタ上でTrailRendererの動作確認をするために有効化しているため、プレイ時に無効化します
			weapon.GetComponentInChildren<TrailRenderer>().enabled = false;
		}

		void Update ()
		{
			//Kobayashi
			if (mixWeight >= 1.0f)
				mixWeight = 1.0f;
			else if (mixWeight <= 0.0f)
				mixWeight = 0.0f;

			// if (Input.GetButtonDown("Fire1"))
			// {
			// 	if (weapon.GetComponent<WeaponController>().IsAttacking())
			// 	{
			// 		//何もしない
			// 	}
			// 	else
			// 	{
			// 		StartCoroutine("AttackCoroutine");
			// 	}
			// }
		}
		
		// IEnumerator AttackCoroutine()
		// {
		// 	weapon.GetComponent<WeaponController>().AttackStart();
		// 	weapon.GetComponentInChildren<TrailRenderer>().enabled = true;
		//
		// 	yield return new WaitForSeconds(attackWaitTime);
		//
		// 	//武器の角度を変える
		// 	weapon.transform.Rotate(-100f, 0.0f, 0.0f);
		// 	
		// 	weapon.GetComponentInChildren<TrailRenderer>().enabled = false;
		// 	weapon.GetComponent<WeaponController>().AttackEnd();
		// }


		// void OnAnimatorIK (int layerIndex)
		// {
		// 	if (isIkActive) {
		// 		anim.SetIKPositionWeight (AvatarIKGoal.RightHand, mixWeight);
		// 		anim.SetIKRotationWeight (AvatarIKGoal.RightHand, mixWeight);
		// 		anim.SetIKPosition (AvatarIKGoal.RightHand, targetObj.position);
		// 		anim.SetIKRotation (AvatarIKGoal.RightHand, targetObj.rotation);
		// 	}
		// }

		void OnAnimatorIK (int layerIndex)
		{
			if (isIkActive) {
				anim.SetIKPositionWeight (AvatarIKGoal.RightHand, mixWeight);
				anim.SetIKRotationWeight (AvatarIKGoal.RightHand, mixWeight);
				anim.SetIKPosition (AvatarIKGoal.RightHand, targetObj.position);
		
				if (Input.GetButtonDown("Fire1"))
				{
					if (weapon.GetComponent<WeaponController>().IsAttacking())
					{
						//何もしない
					}
					else
					{
						StartCoroutine("AttackCoroutine");
					}
				}
				else
				{
					anim.SetIKRotation (AvatarIKGoal.RightHand, targetObj.rotation);
				}
			}
		}
		
		IEnumerator AttackCoroutine()
		{
			weapon.GetComponent<WeaponController>().AttackStart();
			weapon.GetComponentInChildren<TrailRenderer>().enabled = true;
		
			yield return new WaitForSeconds(attackWaitTime);
		
			//角度を変えたオブジェクトと角度を合わせることで剣を振る
			anim.SetIKRotation (AvatarIKGoal.RightHand, targetObjRotation.rotation);
			
			weapon.GetComponentInChildren<TrailRenderer>().enabled = false;
			weapon.GetComponent<WeaponController>().AttackEnd();
		}

		// void OnGUI ()
		// {
		// 	Rect rect1 = new Rect (10, Screen.height - 20, 400, 30);
		// 	isIkActive = GUI.Toggle (rect1, isIkActive, "IK Active");
		// }

	}
}