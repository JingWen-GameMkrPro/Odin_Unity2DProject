using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TestMessages;

public class ProtoTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var a = new TetsMessage();
        a.I1 = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
