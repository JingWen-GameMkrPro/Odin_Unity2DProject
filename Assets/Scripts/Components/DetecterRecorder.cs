using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DetecterRecorder : MonoBehaviour
{

    [HideInInspector]
    public string DetecterName;

    [HideInInspector]
    public List<OuterInteracterBase> RangeObjects = new();

    [SerializeField]
    public string DetectTargetTag;


    private void Start()
    {
        if(DetectTargetTag =="")
        {
            Debug.LogError("Detecter's Info is null, please check it.");
        }

        DetecterName = gameObject.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DetectTargetTag))
        {
            Debug.Log(collision.gameObject.name);
            RangeObjects.Add(getParentObjectOuterInteracter(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(DetectTargetTag))
        {
            RangeObjects.Remove(getParentObjectOuterInteracter(collision));
        }
    }

    private OuterInteracterBase getParentObjectOuterInteracter(Collider2D collision)
    {
        // Àò¨úroot object
        var outerInteracter = collision.transform.root.gameObject.GetComponent<OuterInteracterBase>();
        if(outerInteracter != null)
        {
            return outerInteracter;
        }
        else
        {
            return null;
        }
    }
}
