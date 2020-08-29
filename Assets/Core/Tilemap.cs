using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
    public event EventHandler OnLoaded;
    int[,] ground;
    public Grid<GameTile> grid;
    public List<GameTile> walkableTiles;

    public Tilemap(System.Random randomNumber)
    {
        walkableTiles = new List<GameTile>();
        grid = MapGenerator.Instance.GenerateMap(randomNumber);
        ground = MapGenerator.Instance.map;
        new Pathfinding(grid);
    }

    public void SetTileType(Vector3 worldPosition, GameTile.Type tileType)
    {
        GameTile tilemapObject = grid.GetCellObject(worldPosition);
        if (tilemapObject != null)
        {
            tilemapObject.SetTileType(tileType);
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
            return true;
        }

        // Check left
        if (!GameTileIsWalkable(x - 1, y) &&
       !GameTileIsWalkable(x - 1, y - 1) &&
       !GameTileIsWalkable(x - 1, y + 1))
        {
            SpawnRope(x - 1, y - 1, ropeLength);
            return true;
        }
        //check right
        if (!GameTileIsWalkable(x + 1, y) &&
       !GameTileIsWalkable(x + 1, y - 1) &&
       !GameTileIsWalkable(x + 1, y + 1))
        {
            SpawnRope(x + 1, y - 1, ropeLength);
            return true;
        }
        //check above
        for (int i = ropeLength; i > 0; i--)
        {
            if (ground.GetLength(1) > y + i)
                if (ground[x, y + i] == 0)
                {
                    SpawnRope(x, y + i, ropeLength);
                    return true;
                }
        }

        return false;
    }
    public void SpawnRope(int x, int y, int ropeLength)
    {
        for (int i = 0; i < ropeLength; i++)
        {
            GameTile tile = grid.GetCellObject(x, y - i);
            if (tile.walkable == false)
            {
                tile.SetIsWalkable(true);
                tile.SetTileType(GameTile.Type.RopeMiddle);
            }
        }
    }

    public Vector3[] GetEnemyPositions(int amount)
    {
        Vector3[] positions = new Vector3[amount];
        for (int i = 0; i < amount; i++)
        {
            GameTile tile = walkableTiles[UnityEngine.Random.Range(0, walkableTiles.Count)];

            while (tile.occupied || tile.y == grid.GetHeight() - 1)
            {
                tile = walkableTiles[UnityEngine.Random.Range(0, walkableTiles.Count)];
            }
            positions[i] = grid.GetWorldPosition(tile.x, tile.y);
            tile.occupied = true;
        }
        return positions;
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
