using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class = 箱子的設計圖
public class BloodItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root.gameObject.CompareTag("Player"))
        {
            Debug.Log("開始回血");
            GameObject playerObject = collision.transform.root.gameObject;

            GameMaster.Instance.OuterInteracterPlayer.AddHP(this.transform.gameObject, 5f);
        }
    }
    
}

