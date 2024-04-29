using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Actor外在神經Base: 所有角色之間的所有互動行為都在這邊
public abstract class OuterInteracterBase : MonoBehaviour
{
    //自己本身
    GameObject Causer;

    private void Start()
    {
        Causer = transform.root.gameObject;
    }

    //扣血
    public virtual void ReduceHP(GameObject causer, float value) 
    {
        Debug.Log("Reduce HP");
    }


}
