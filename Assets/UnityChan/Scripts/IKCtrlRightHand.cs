//
//IKCtrlRightHand.cs
//
//Sample script for IK Control of Unity-Chan's right hand.
//
//2014/06/20 N.Kobayashi
//
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
		[SerializeField] private  bool isIkActive = false;
		[SerializeField] private  float mixWeight = 1.0f;

		void Awake ()
		{
			anim = GetComponent<Animator> ();
		}

		void Update ()
		{
			//Kobayashi
			if (mixWeight >= 1.0f)
				mixWeight = 1.0f;
			else if (mixWeight <= 0.0f)
				mixWeight = 0.0f;
		}

		void OnAnimatorIK (int layerIndex)
		{
			if (isIkActive) {
				anim.SetIKPositionWeight (AvatarIKGoal.RightHand, mixWeight);
				anim.SetIKRotationWeight (AvatarIKGoal.RightHand, mixWeight);
				anim.SetIKPosition (AvatarIKGoal.RightHand, targetObj.position);

				if (Input.GetButton("Fire1"))
				{
					//角度を変えたオブジェクトと角度を合わせることで剣を振る
					anim.SetIKRotation (AvatarIKGoal.RightHand, targetObjRotation.rotation);
					isAttacking = true;
				}
				else
				{
					anim.SetIKRotation (AvatarIKGoal.RightHand, targetObj.rotation);
					isAttacking = false;
				}
			}
		}
		
		// void OnGUI ()
		// {
		// 	Rect rect1 = new Rect (10, Screen.height - 20, 400, 30);
		// 	isIkActive = GUI.Toggle (rect1, isIkActive, "IK Active");
		// }

	}
}