using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Initial : ActionNode
{

    protected override void OnStart() 
    {
        //go = blackboard.Find<GameObject>("RefPlayer").value;

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
