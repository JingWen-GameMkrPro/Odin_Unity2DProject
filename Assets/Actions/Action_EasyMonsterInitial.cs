using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_EasyMonsterInitial : ActionNode
{
    public NodeProperty<GameObject> RefPlayer;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        RefPlayer.Value = GameObject.FindGameObjectWithTag("Player");

        return State.Success;
    }
}
