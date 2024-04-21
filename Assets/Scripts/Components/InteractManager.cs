using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 這個腳本為玩家與怪物互動的中間元件
/// </summary>
public class InteractManager : MonoBehaviour
{
    private AttributeManager attributeManager;
    private ColliderManager colliderManager;


    void Start()
    {
        //獲得AttribuManager
        attributeManager = GetComponent<AttributeManager>();
        if(attributeManager == null )
        {
            Debug.LogError("attributeManager == null, please check it.");
        }

        //獲得ColliderManager
        colliderManager = GetComponent<ColliderManager>();
        if (colliderManager == null)
        {
            Debug.LogError("colliderManager == null, please check it.");
        }
    }

    //造成傷害
    public void Damage(float damageValue, string targetDetecterName)
    {
        if (colliderManager.ColliderDetecterDictionary.ContainsKey(targetDetecterName))
        {
            var interactManagers = getInteractManagerfromObject(colliderManager.ColliderDetecterDictionary[targetDetecterName]);
            foreach(var interactManager in interactManagers)
            {
                interactManager.BeDamaged(damageValue);
            }
        }
    }

    //將Detecter的GameObject身上獲取InteractManager
    private List<InteractManager> getInteractManagerfromObject(List<GameObject> gameObjects)
    {
        var interactManagers = new List<InteractManager>();
        foreach (var objectItem in gameObjects)
        {
            var interactManager = objectItem.GetComponent<InteractManager>();
            if (interactManager != null)
            {
                interactManagers.Add(interactManager);
            }
        }
        return interactManagers;
    }

    //受到傷害
    public void BeDamaged(float damageValue)
    {
        attributeManager.ReduceHP(damageValue);
    }

    //使彈開
    public void BeBounceUp()
    {

    }

    //使暈眩
    public void BeDizzy()
    {

    }
}
