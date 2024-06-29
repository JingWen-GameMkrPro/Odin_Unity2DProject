using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_EasyMonsterFindPlayerPosition : ActionNode
{
    public NodeProperty<string> Message;
    public NodeProperty<GameObject> Player;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        Message.Value = Player.Value.transform.position.ToString();
        return State.Success;
    }
}
