using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    [SerializeField]
    public AttributeData sourceAttributeData;

    private AttributeData.Data currentAttributeData;
    private void Start()
    {
        currentAttributeData = sourceAttributeData.data;
    }

    public void ReduceHP(float value)
    {
        currentAttributeData.HP = Math.Clamp((currentAttributeData.HP-value), 0, currentAttributeData.maxHP);
        Debug.Log("ReduceHP...current hp = "+ currentAttributeData.HP);
    }

    public void AddHP(float value)
    {
        currentAttributeData.HP = Math.Clamp((currentAttributeData.HP + value), 0, currentAttributeData.maxHP);
    }
}
