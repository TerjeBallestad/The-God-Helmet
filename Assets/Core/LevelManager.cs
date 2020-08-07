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
    private FloorLean lean;

    private void Start()
    {
        lean = FloorLean.Left;
        for (int i = 0; i < floors; i++)
        {
            Tilemap tilemap = new Tilemap(width, 1, 1f, Vector3.zero);
            TilemapVisual visual = Instantiate(tilemapVisual);
            tilemap.SetTilemapVisual(visual);
            float rotation = 0;
            switch (lean)
            {
                case FloorLean.Left:
                    rotation = 2f;
                    lean = FloorLean.Right;
                    break;
                case FloorLean.Right:
                    rotation = -2f;
                    lean = FloorLean.Left;
                    break;
                case FloorLean.Straight:
                    break;
                default:
                    break;
            }
            visual.transform.Rotate(0, 0, rotation, Space.World);
            visual.transform.Translate(0, i * distanceBetweenFloors - 0.2f, 0, Space.World);
            for (int x = 0; x < tilemap.grid.GetWidth(); x++)
            {
                for (int y = 0; y < tilemap.grid.GetHeight(); y++)
                {
                    Tilemap.Tile tile = tilemap.grid.GetCellObject(x, y);
                    tile.SetTileSprite(Tilemap.Tile.Sprite.Path);
                }
            }
        }
    }
}

public class LevelTile
{

}
