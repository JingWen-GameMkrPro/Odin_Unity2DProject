using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void DetectRedpotionId(RedPotion redpotion)
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            redpotion.Interact(this);
        }
    } 

    public void ReceiveRedpotionId(string Redpotion)
    {
        Debug.Log("使用了" +Redpotion);
        GameMaster.Instance.PlayerDatabase.PlayerAtt.HP = GameMaster.Instance.PlayerDatabase.PlayerAtt.HP + 10;
    }
}