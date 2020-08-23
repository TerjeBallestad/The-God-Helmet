using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private TilemapVisual tilemapVisual;
    public string seed;
    public bool randomSeed = true;
    private Grid<GameTile> gameLogic;
    private Tilemap tilemap;
    private List<TilemapVisual> sprites;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (randomSeed)
        {
            seed = DateTime.Now.Ticks.ToString();
        }
        System.Random randomNumber = new System.Random(seed.GetHashCode());
        tilemap = new Tilemap(randomNumber);

    }
}
