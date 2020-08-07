using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum FloorLean
    {
        Straight,
        Left,
        Right,
    }
    [SerializeField] private TilemapVisual tilemapVisual;
    public int floors = 5;
    public int width = 20;
    public int distanceBetweenFloors = 10;
    private Grid<LevelTile> GameLogic;
    private Grid<Pathfinding> Pathfinding;
    private Grid<TilemapVisual> Sprites;

    private void Start()
    {
        SpawnFloors();
    }

    private void SpawnFloors()
    {
        FloorLean lean = FloorLean.Left;

        for (int i = 0; i < floors; i++)
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
        SpawnFloor(FloorLean.Straight, floors);
    }
    private void SpawnFloor(FloorLean lean, int index)
    {
        Tilemap tilemap = new Tilemap(width, 1, 1f, Vector3.zero);
        TilemapVisual visual = Instantiate(tilemapVisual);
        tilemap.SetTilemapVisual(visual);
        float rotation = 0;
        float xPosition = 0;

        switch (lean)
        {
            case FloorLean.Left:
                rotation = 2f;
                xPosition = 1f;
                break;
            case FloorLean.Right:
                rotation = -2f;
                xPosition = -1f;
                break;
            case FloorLean.Straight:
                rotation = 0;
                xPosition = 0;
                break;
            default:
                break;
        }

        visual.transform.RotateAround(visual.GetComponent<BoxCollider2D>().bounds.center, Vector3.forward, rotation);
        visual.transform.Translate(xPosition, index * distanceBetweenFloors - 0.2f, 0, Space.World);
        bool topFloor = index == floors;
        for (int x = 0; x < tilemap.grid.GetWidth(); x++)
        {
            for (int y = 0; y < tilemap.grid.GetHeight(); y++)
            {
                Tilemap.Tile tile = tilemap.grid.GetCellObject(x, y);
                if (topFloor) tile.SetTileSprite(Tilemap.Tile.Sprite.Grass);
                else tile.SetTileSprite(Tilemap.Tile.Sprite.Path);
            }
        }
    }
}

public class LevelTile
{

}
