using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_SceneStateController
{
    ISceneState currentSceneState;
    bool isCurrentSceneStateInitial = false;

    public Script_SceneStateController()
    {

    }

    public void SetSceneState(ISceneState inputSceneState)
    {
        LoadScene(inputSceneState.sceneStateName);

        if(currentSceneState != null) 
        {
            currentSceneState.StateEnd();
        }
        currentSceneState = inputSceneState;
        isCurrentSceneStateInitial = false;
    }

    public void LoadScene(string inputSceneName)
    {
        Debug.Log($"Loading \"{inputSceneName}\"");
        SceneManager.LoadScene(inputSceneName);
    }

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

public class ISceneState
{
    public string sceneStateName = "NoneStateName";

    protected Script_SceneStateController currentSceneStateController = null;

    public ISceneState(Script_SceneStateController inputSceneStateController)
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


public class SceneState_MainMenu: ISceneState
{
    //Sample
    int processValue = 1000;

    public SceneState_MainMenu(Script_SceneStateController inputSceneStateController): base(inputSceneStateController) 
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
            currentSceneStateController.SetSceneState(new SceneState_Endless0(currentSceneStateController));
        }


    }



}

public class SceneState_Endless0: ISceneState
{
    public SceneState_Endless0(Script_SceneStateController inputSceneStateController) : base(inputSceneStateController) 
    {
        sceneStateName = "Scene_Endless0";
    }

    public override void StateUpdate()
    {
        Debug.Log($"Current we are in the scene \"{sceneStateName}\"");
    }
}

