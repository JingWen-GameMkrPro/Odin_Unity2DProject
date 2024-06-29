using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Detect : ActionNode
{
    private GameObject target;

    protected override void OnStart() 
    {
       // target = blackboard.Find<GameObject>("Target");
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if(target == null)
        {
            return State.Failure;
        }
        else
        {
            return State.Success;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform.root.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
        }
    }

}
