using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class BTA_MoveToEnemy : ActionNode
{
    protected override void OnStart() 
    {

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        var owner = blackboard.Find<GameObject>("Owner").value;
        var targetPosition = GameMaster.Instance.Player.transform.position;
        owner.transform.position = new Vector3(targetPosition.x, targetPosition.y + 2, targetPosition.z);
        Debug.Log("成功跳到上方");
        return State.Success;
    }
}
