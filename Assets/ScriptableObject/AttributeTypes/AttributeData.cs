using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttributeData", menuName = "ScriptableObjects/AttributeData", order = 1)]
public class AttributeData : ScriptableObject
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
