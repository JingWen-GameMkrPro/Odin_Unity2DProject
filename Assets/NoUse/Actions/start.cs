using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ///SceneManager.LoadScene("Scene_Endless0");
        Debug.Log("Start");
    }

    // Update is called once per frame
    public void open()
    {
        Debug.Log("open");
        SceneManager.LoadScene("Scene_Endless0");
    }
}
