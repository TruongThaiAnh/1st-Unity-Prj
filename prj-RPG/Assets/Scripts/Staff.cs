using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        Debug.Log("staff attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
