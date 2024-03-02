using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//Controller: Controll all scenes.
public class Controller_SceneState
{
    private ISceneState currentSceneState;
    private bool isCurrentSceneStateInitial = false;

    //Change scene
    public void ChangeSceneState(ISceneState inputSceneState)
    {
        LoadScene(inputSceneState.sceneStateName);

        if(currentSceneState != null) 
        {
            currentSceneState.StateEnd();
        }
        currentSceneState = inputSceneState;
        isCurrentSceneStateInitial = false;
    }

    //Load scene
    public void LoadScene(string inputSceneName)
    {
        Debug.Log($"Loading \"{inputSceneName}\"");
        SceneManager.LoadScene(inputSceneName);
    }

    //Update scene
    public void SceneStateUpdate()
    {
        if(!SceneManager.GetSceneByName(currentSceneState.sceneStateName).isLoaded)
        {
            return;
        }

        //ªì©l¤Æ
        if(currentSceneState != null && isCurrentSceneStateInitial==false)
        {
            currentSceneState.StateBegin();
            isCurrentSceneStateInitial=true;
        }

        //Update
        if(currentSceneState != null)
        {
            currentSceneState.StateUpdate();
        }
    }
}

//Mother of scene state
public class ISceneState
{
    public string sceneStateName = "NoneStateName";

    protected Controller_SceneState currentSceneStateController = null;

    public ISceneState(Controller_SceneState inputSceneStateController)
    {
        currentSceneStateController = inputSceneStateController;
    }

    public virtual void StateBegin()
    {

    }

    public virtual void StateUpdate()
    {

    }

    public virtual void StateEnd() 
    {
    
    }


}

//Child scnene state
public class SceneState_MainMenu: ISceneState
{
    //Sample
    int processValue = 1000;

    public SceneState_MainMenu(Controller_SceneState inputSceneStateController): base(inputSceneStateController) 
    {
        sceneStateName = "Scene_MainMenu";
    }


    public override void StateUpdate()
    {
        //Sample
        if (processValue > 0) 
        {
            processValue--;
            Debug.Log($"Loading process is {processValue}");
        }
        else
        {
            currentSceneStateController.ChangeSceneState(new SceneState_Endless0(currentSceneStateController));
        }


    }



}

//Child scene state
public class SceneState_Endless0: ISceneState
{
    public SceneState_Endless0(Controller_SceneState inputSceneStateController) : base(inputSceneStateController) 
    {
        sceneStateName = "Scene_Endless0";
    }

    public override void StateUpdate()
    {
        Debug.Log($"Current we are in the scene \"{sceneStateName}\"");
    }
}

