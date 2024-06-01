using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
     public float currentTime;
     bool time = true;
    // Start is called before the first frame update
    void Start()
    {
        bool time = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameMaster.Instance.PlayerDatabase.PlayerAtt.HP>0)
        {
            currentTime +=Time.fixedDeltaTime;
        }
        else if(GameMaster.Instance.PlayerDatabase.PlayerAtt.HP==0)
        {
            bool time = false;
            Debug.Log(currentTime);
        }
    }
}
