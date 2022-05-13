using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemSlot : MonoBehaviour
{
    public Transform parentOverride;
    public GameObject currentWeaponModel;

    private void UnloadWeapon()
    {
        if (currentWeaponModel != null) currentWeaponModel.SetActive(false);
    }

    private void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null) Destroy(currentWeaponModel);
    }

    private void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();
        
        if (weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        var model = Instantiate(weaponItem.modelPrefab) as GameObject;
        
        if (model != null)
        {
            model.transform.parent = parentOverride != null ? parentOverride : transform;
            
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }
}
