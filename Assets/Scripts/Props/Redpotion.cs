using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPotion : MonoBehaviour
{
    public string potion;
    public void Interact(Player player)
    {
       player.ReceiveRedpotionId(potion);
        Destroy(gameObject);
    }
}
