using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttributeTypes", menuName = "ScriptableObjects/AttributeTypes", order = 1)]
public class AttributeTypes : ScriptableObject
{
    [Serializable]
    public struct Data
    {
        public float maxHP;
        public float HP;
        public float originSpeedWeight;
        public float speedWeight;
    }
    [SerializeField]
    public Data data = new();
}
