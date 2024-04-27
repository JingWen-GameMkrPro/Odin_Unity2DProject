using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInnerInteracter : MonoBehaviour
{
    [SerializeField]
    public PlayerBehavior PlayerBehavior;

    private void Update()
    {
        PlayerBehavior.CheckGrounded();

        PlayerBehavior.TryHorizontalMove();

        PlayerBehavior.UpdateaAnimatorState();

    }

}
