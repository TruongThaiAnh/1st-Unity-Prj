using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon 
{
  public void Attack()
    {
        Debug.Log("bow attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
