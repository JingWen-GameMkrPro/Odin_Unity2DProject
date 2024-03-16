using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public List<Collider2D> hitMonsters = new();    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if (collision.CompareTag("HitBox_Monster"))
        {
            hitMonsters.Add(collision);
            //currentDamageSystem.CauseDamage()
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox_Monster"))
        {
            hitMonsters.Remove(collision);
            //currentDamageSystem.CauseDamage()
        }
    }



}
