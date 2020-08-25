using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
    public event EventHandler OnLoaded;
    int[,] ground;
    public Grid<GameTile> grid;

    public Tilemap(System.Random randomNumber)
    {
        grid = MapGenerator.Instance.GenerateMap(randomNumber);
        ground = MapGenerator.Instance.map;
        new Pathfinding(grid);
    }

    public void SetTilemapSprite(Vector3 worldPosition, GameTile.Sprite tilemapSprite)
    {
        GameTile tilemapObject = grid.GetCellObject(worldPosition);
        if (tilemapObject != null)
        {
            tilemapObject.SetTileSprite(tilemapSprite);
        }
    }

    public void SetTilemapVisual(TilemapVisual tilemapVisual)
    {
        tilemapVisual.SetGrid(this, grid);
    }

    public bool SpawnRope(Vector3 worldPosition, int ropeLength)
    {
        int x = grid.GetCellObject(worldPosition).x;
        int y = grid.GetCellObject(worldPosition).y;
        //check below
        if (!GameTileIsWalkable(x, y - 1) && y - 1 > 0 && ground[x, y - 1] == 0)
        {
            SpawnRope(x, y - 1, ropeLength);
        }

        // Check left
        else if (!GameTileIsWalkable(x - 1, y) &&
        !GameTileIsWalkable(x - 1, y - 1) &&
        !GameTileIsWalkable(x - 1, y + 1))
        {
            Debug.Log("Found available spot at " + x + ", " + y);
            SpawnRope(x - 1, y - 1, ropeLength);
            return true;
        }
        //check right
        else if (!GameTileIsWalkable(x + 1, y) &&
        !GameTileIsWalkable(x + 1, y - 1) &&
        !GameTileIsWalkable(x + 1, y + 1))
        {
            Debug.Log("Found available spot at " + x + ", " + y);
            SpawnRope(x + 1, y - 1, ropeLength);
            return true;
        }
        //check above
        for (int i = ropeLength; i > 0; i--)
        {
            if (ground.GetLength(1) >= y + i && ground[x, y + i] == 0)
            {
                Debug.Log("Found spot up above, at " + x + ", " + y + ropeLength);
                SpawnRope(x, y + i, ropeLength);
                return true;
            }
        }



        return false;
    }
    public void SpawnRope(int x, int y, int ropeLength)
    {
        Debug.Log(ropeLength);
        for (int i = 0; i < ropeLength; i++)
        {
            Debug.Log("setting walkable at " + x + "and " + (y - i));
            if (grid.GetCellObject(x, y - i).walkable == false)
                grid.GetCellObject(x, y - i).SetIsWalkable(true);
        }
    }

    public bool GameTileIsWalkable(int x, int y)
    {
        GameTile tile = grid.GetCellObject(x, y);
        if (tile != null)
        {
            return tile.walkable;
        }
        else return false;
    }

    /*
     * Save - Load
     * */
    public class SaveObject
    {
        public GameTile.SaveObject[] tileSaveObjects;
    }

    public void Save()
    {
        List<GameTile.SaveObject> tileSaveObjects = new List<GameTile.SaveObject>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                GameTile tilemapObject = grid.GetCellObject(x, y);
                tileSaveObjects.Add(tilemapObject.Save());
            }
        }
        SaveObject saveObject = new SaveObject { tileSaveObjects = tileSaveObjects.ToArray() };

        SaveSystem.SaveObject(saveObject);
    }

    public void Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        foreach (GameTile.SaveObject tileSaveObject in saveObject.tileSaveObjects)
        {
            GameTile tilemapObject = grid.GetCellObject(tileSaveObject.x, tileSaveObject.y);
            tilemapObject.Load(tileSaveObject);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }
}
