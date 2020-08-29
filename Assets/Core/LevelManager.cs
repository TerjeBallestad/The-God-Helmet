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
    public int treasure = 20;
    public Tilemap tilemap { get; private set; }
    private Grid<GameTile> gameLogic;
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
        tilemap.SetTilemapVisual(tilemapVisual);

        for (int x = 0; x < tilemap.grid.GetWidth(); x++)
        {
            for (int y = 0; y < tilemap.grid.GetHeight(); y++)
            {
                GameTile tile = tilemap.grid.GetCellObject(x, y);
                if (tile.walkable)
                {
                    tilemap.walkableTiles.Add(tile);
                }
            }
        }

        EnemyManager.Instance.SpawnEvilMinons();
    }

}
