using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public enum FloorLean
    {
        Straight,
        Left,
        Right,
    }
    [SerializeField] private TilemapVisual tilemapVisual;
    public bool showDebugGrid = true;
    public int floors = 5;
    public int width = 20;
    public int distanceBetweenFloors = 10;
    private Grid<GameTile> gameLogic;
    private Tilemap tilemap;
    private List<TilemapVisual> sprites;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CreateTilemap();
        SpawnFloors();
    }

    private void SpawnFloors()
    {
        FloorLean lean = FloorLean.Left;

        for (int i = 0; i <= floors; i++)
        {
            SpawnFloor(lean, i);

            switch (lean)
            {
                case FloorLean.Right:
                    lean = FloorLean.Left;
                    break;
                case FloorLean.Left:
                    lean = FloorLean.Right;
                    break;
                default:
                    break;
            }
        }
    }
    private void SpawnFloor(FloorLean lean, int index)
    {
        float rotation = 0;
        int xPosition = 0;

        switch (lean)
        {
            case FloorLean.Left:
                rotation = 2f;
                xPosition = 3;
                break;
            case FloorLean.Right:
                rotation = -2f;
                xPosition = 1;
                break;
            case FloorLean.Straight:
                rotation = 0;
                xPosition = 0;
                break;
            default:
                break;
        }
        // Tilemap tilemap = new Tilemap(width, 1, 1f, new Vector3(xPosition, index * distanceBetweenFloors));
        // visual.transform.RotateAround(visual.GetComponent<BoxCollider2D>().bounds.center, Vector3.forward, rotation);
        // visual.transform.Translate(xPosition, 0, 0, Space.World);
        bool topFloor = index == floors;
        bool bottomFloor = index == 0;
        if (bottomFloor)
        {
            for (int x = 1; x < tilemap.grid.GetWidth() - 1; x++)
            {
                GameTile tile = tilemap.grid.GetCellObject(x, 0);
                tile.SetTileSprite(GameTile.Sprite.Path);
                tile.SetIsWalkable(true);
            }
        }
        else
        {
            for (int x = xPosition; x < width + xPosition; x++)
            {
                GameTile tile = tilemap.grid.GetCellObject(x, index * distanceBetweenFloors);
                tile.SetIsWalkable(true);
                if (topFloor) tile.SetTileSprite(GameTile.Sprite.Grass);
                else tile.SetTileSprite(GameTile.Sprite.Path);
            }
        }
        // sprites.Add(visual);
    }
    private void CreateTilemap()
    {
        tilemap = new Tilemap(width + 4, floors * distanceBetweenFloors + 3, 1f, Vector3.zero, showDebugGrid);
        // TilemapVisual visual = Instantiate(tilemapVisual);
        tilemap.SetTilemapVisual(tilemapVisual);

        // Create walls
        for (int y = 0; y < floors * distanceBetweenFloors; y++)
        {
            tilemap.grid.GetCellObject(0, y).SetTileSprite(GameTile.Sprite.Ground);
            tilemap.grid.GetCellObject(width + 3, y).SetTileSprite(GameTile.Sprite.Ground);
        }
    }
}
