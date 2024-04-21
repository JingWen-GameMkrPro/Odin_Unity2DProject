using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField]
    public List<ColliderDetecter> ColliderDetecterList = new List<ColliderDetecter>();

    public Dictionary<string, List<GameObject>> ColliderDetecterDictionary = new Dictionary<string, List<GameObject>>();

    private void Start()
    {
        foreach(var detecterItem in ColliderDetecterList)
        {
            ColliderDetecterDictionary.Add(detecterItem.DetecterName, detecterItem.rangeObjects);
        }
    }


}
