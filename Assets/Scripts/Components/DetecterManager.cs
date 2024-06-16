using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DetecterManager : MonoBehaviour
{
    [SerializeField]
    public List<DetecterRecorder> DetecterRecorderOriginList = new List<DetecterRecorder>();

    [HideInInspector]
    public Dictionary<string, List<OuterInteracterBase>> DetecterRecorderList = new();

    private void Start()
    {
        foreach(var detecterRecorder in DetecterRecorderOriginList)
        {
            DetecterRecorderList.Add(detecterRecorder.gameObject.name, detecterRecorder.RangeObjects);
        }
    }


}
