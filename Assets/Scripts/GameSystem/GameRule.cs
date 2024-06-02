using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    { 
        if(GameMaster.Instance.PlayerDatabase.PlayerAtt.HP>0)
        {
            // Debug.Log(GameMaster.Instance.PlayerDatabase.PlayerAtt.HP);
        }else if(GameMaster.Instance.PlayerDatabase.PlayerAtt.HP==0)
        {
            Debug.Log("Game Over");
        }
    }
}

    
    

