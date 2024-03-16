using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageTypes", menuName = "ScriptableObjects/DamageTypes", order = 1)]
public class DamageTypes : ScriptableObject
{
    
    [Serializable]
    public class DamageType
    {
        public string DamageName;
        public float DamageValue;
        public string colliderName;
    }
    public List<DamageType> DamageDictionary = new();


}
