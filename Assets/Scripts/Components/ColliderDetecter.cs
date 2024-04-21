using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ColliderDetecter : MonoBehaviour
{
    [SerializeField]
    public string DetecterName = ""; //e.g. Attack
    public string DetectTag = "";

    [NonSerialized]
    public List<GameObject> rangeObjects = new();

    private void Start()
    {
        if(DetectTag =="" || DetecterName == "")
        {
            Debug.LogError("SerializeField is Null, Please check it.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DetectTag))
        {
            rangeObjects.Add(getParentObjectfromCollider(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(DetectTag))
        {
            rangeObjects.Remove(getParentObjectfromCollider(collision));
        }
    }

    private GameObject getParentObjectfromCollider(Collider2D collision)
    {
        // Àò¨úroot object
        return collision.transform.root.gameObject;
    }
}
