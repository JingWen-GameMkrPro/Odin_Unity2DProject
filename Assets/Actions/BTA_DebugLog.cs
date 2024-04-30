using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class BTA_DebugLog : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        var t = blackboard.Find<MonsterDatabase>("Database");
        if(t.value.DetecterManager.DetecterRecorderList.ContainsKey("PatrolDetecter"))
        {
            Debug.Log(t.value.DetecterManager.DetecterRecorderList["PatrolDetecter"].Count);
           
        }
        else
        {
            Debug.Log("wait...");
        }
        
        return State.Success;
    }
}
