using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{

    public event EventHandler OnLoaded;

    public Grid<Tile> grid;

    public Tilemap(int width, int height, float cellSize, Vector3 originPosition)
    {
        grid = new Grid<Tile>(width, height, cellSize, originPosition, (Grid<Tile> g, int x, int y) => new Tile(g, x, y));
    }

    public void SetTilemapSprite(Vector3 worldPosition, Tile.Sprite tilemapSprite)
    {
        Tile tilemapObject = grid.GetCellObject(worldPosition);
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
        public Tile.SaveObject[] tileSaveObjects;
    }

    public void Save()
    {
        List<Tile.SaveObject> tileSaveObjects = new List<Tile.SaveObject>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Tile tilemapObject = grid.GetCellObject(x, y);
                tileSaveObjects.Add(tilemapObject.Save());
            }
        }

        SaveObject saveObject = new SaveObject { tileSaveObjects = tileSaveObjects.ToArray() };

        SaveSystem.SaveObject(saveObject);
    }

    public void Load()
    {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        foreach (Tile.SaveObject tileSaveObject in saveObject.tileSaveObjects)
        {
            Tile tilemapObject = grid.GetCellObject(tileSaveObject.x, tileSaveObject.y);
            tilemapObject.Load(tileSaveObject);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }



    /*
     * Represents a single Tilemap Object that exists in each Grid Cell Position
     * */
    public class Tile
    {
        public enum Sprite
        {
            None,
            Ground,
            Path,
            Dirt,
            Background,
        }

        private Grid<Tile> grid;
        private int x;
        private int y;
        private Sprite sprite;

        public Tile(Grid<Tile> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTileSprite(Sprite tilemapSprite)
        {
            this.sprite = tilemapSprite;
            grid.TriggerCellObjectChanged(x, y);
        }

        public Sprite GetTilemapSprite()
        {
            return sprite;
        }

        public override string ToString()
        {
            return sprite.ToString();
        }



        [System.Serializable]
        public class SaveObject
        {
            public Sprite tilemapSprite;
            public int x;
            public int y;
        }

        /*
         * Save - Load
         * */
        public SaveObject Save()
        {
            return new SaveObject
            {
                tilemapSprite = sprite,
                x = x,
                y = y,
            };
        }

        public void Load(SaveObject saveObject)
        {
            sprite = saveObject.tilemapSprite;
        }
    }
}
