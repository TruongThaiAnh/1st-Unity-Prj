using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySLot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    
    public WeaponInfo GetWeaponInfo() { return weaponInfo; }


}
