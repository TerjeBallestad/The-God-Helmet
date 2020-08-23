using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{

    public event EventHandler OnLoaded;

    public Grid<GameTile> grid;

    public Tilemap(System.Random randomNumber, bool showDebug = true)
    {
        grid = MapGenerator.Instance.GenerateMap(randomNumber, showDebug);
        // grid = new Grid<GameTile>(width, height, originPosition, (Grid<GameTile> g, int x, int y) => new GameTile(g, x, y), showDebug);
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
