using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum Turn
    {
        Player,
        Opponent,
    }
    public Turn turn;
    public MinionBase selectedMinion;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        turn = Turn.Player;
    }

}
