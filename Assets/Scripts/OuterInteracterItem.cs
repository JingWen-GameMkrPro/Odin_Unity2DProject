using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OuterInteracterItem : OuterInteracterBase
{
 public string RedPoison;
 public int id= 1;
 public void Detecter_Item()
 {
    Debug.Log("我拿到紅藥水了");
    GameMaster.Instance.PlayerDatabase.PlayerAtt.HP += 1;
 }
}
