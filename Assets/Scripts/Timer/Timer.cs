using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int time;
    public bool StopTimer = false;

     void Start()
     {
       StartCoroutine(GameTimer());
     }
    
    IEnumerator GameTimer()
     {
      while (StopTimer ==false)
       {
        time+=1;
        yield return new WaitForSeconds(1f);
        if(GameMaster.Instance.PlayerDatabase.PlayerAtt.HP==0)
          {
            StopTimer = true;
            Debug.Log("目前時間"+time);
          }
       }
 
     }
}
