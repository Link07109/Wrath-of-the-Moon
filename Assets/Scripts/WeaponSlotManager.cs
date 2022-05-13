using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot handSlot;

    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        handSlot = GetComponentInChildren<WeaponHolderSlot>();
        
        handSlot.LoadWeaponModel(weaponItem);
    }
}
