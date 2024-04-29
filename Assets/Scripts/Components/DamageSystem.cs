using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField]
    public  AttributeManager currentAttributeSystem;
    public DamageTypes currentDamageTypes;
    public DetecterManager currentColliderManager;
    /*
    public void CauseDamage(string damageName, DamageSystem targetDamageSystem)
    {
        var damageType = currentDamageTypes.DamageDictionary.FirstOrDefault(x => x.DamageName == damageName);
        
        foreach(var item in currentColliderManager.hitMonsters)
        {
            var t = item.GetComponentInParent<DamageSystem>();
            t.BeDamaged(damageType.DamageValue, this);
        }
        
    }

    public void BeDamaged(float damageValue, DamageSystem sourceDamageSystem)
    {
        currentAttributeSystem.ReduceHP(damageValue);
    }

    public void ReceiveTarget(DamageSystem targetDamageSystem)
    {
        //CauseDamage()
    }
    */
}
