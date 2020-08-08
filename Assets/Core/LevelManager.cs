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
    private Grid<LevelTile> gameLogic;
    private Pathfinding pathfinding;
    private List<TilemapVisual> sprites;

    private void Start()
    {
        SpawnFloors();
        CreatePathfinding();
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
        Tilemap tilemap = new Tilemap(width, 1, 1f, new Vector3(xPosition, index * distanceBetweenFloors));
        TilemapVisual visual = Instantiate(tilemapVisual);
        tilemap.SetTilemapVisual(visual);
        visual.transform.RotateAround(visual.GetComponent<BoxCollider2D>().bounds.center, Vector3.forward, rotation);
        // visual.transform.Translate(xPosition, 0, 0, Space.World);
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
        sprites.Add(visual);
    }
    private void CreatePathfinding()
    {
        pathfinding = new Pathfinding(width, floors * distanceBetweenFloors);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < floors * distanceBetweenFloors; y += distanceBetweenFloors)
            {

                pathfinding.GetNode(Mathf.Clamp(x, 1, width - 2), y).SetIsWalkable(false);
            }
        }
    }
}


public class LevelTile
{

}
