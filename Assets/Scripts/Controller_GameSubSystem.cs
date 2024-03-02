using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_GameSubSystem
{
    //Manage system as below:
    private GameJudgeSystem _gameJudgeSystem;
    private GameTimeSystem _gameTimeSystem;
    private MonsterGenerateSystem _monsterGenerateSystem;
    private LevelJudgeSystem _levelJudgeSystem;


    public void Initial()
    {
        _gameJudgeSystem = new GameJudgeSystem();
        _gameTimeSystem = new GameTimeSystem();
        _monsterGenerateSystem = new MonsterGenerateSystem();
        _levelJudgeSystem = new LevelJudgeSystem();
    }

    public void Update()
    {
        _gameJudgeSystem.Update();
        _gameTimeSystem.Update();
        _monsterGenerateSystem.Update();
        _levelJudgeSystem.Update();
    }

    //Timer
    public void puaseTimer()
    {

    }

    public void continueTimer()
    {

    }
    //Monster Generate
    public void generateCurrentLevelMonsterList()
    {

    }
   
    //Level Judge
    public void nextLevel()
    {

    }

    //Game Judge
    public bool isGameOver()
    {
        return false;
    }
}
