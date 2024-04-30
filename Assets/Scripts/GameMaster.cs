using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster
{
    private GameMaster() {}

    public static GameMaster Instance { get; } = new();

    public GameObject Player;
    public OuterInteracterPlayer OuterInteracterPlayer;
    

}
