using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    //Controll all scenes
    Controller_SceneState _currentSceneStateController = new Controller_SceneState();
    //Controll all game systems
    Controller_GameSubSystem _currentGameSubsystemController = new Controller_GameSubSystem();

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        //Change to main menu scene
        _currentSceneStateController.ChangeSceneState(new SceneState_MainMenu(_currentSceneStateController));
        _currentGameSubsystemController.Initial();


    }

    // Update is called once per frame
    void Update()
    {
        _currentSceneStateController.SceneStateUpdate();
        _currentGameSubsystemController.Update();
    }
}
