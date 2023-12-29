using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController;
    public bool IsAttacking()
    {
        return weaponController.IsAttacking();
    }
}
