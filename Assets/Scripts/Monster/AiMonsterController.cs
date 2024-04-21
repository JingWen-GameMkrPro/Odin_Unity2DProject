using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMonsterController : MonoBehaviour
{
    private GameObject playerObject;

    private void Start()
    {
        //直接先獲得玩家物件訊息
        playerObject = getPlayerObject();

    }

    private void FixedUpdate()
    {
        
    }


    private GameObject getPlayerObject()
    {
        return GameObject.Find("Character_Main"); ;
    }

    private void horizontalMove()
    {

    }


}
