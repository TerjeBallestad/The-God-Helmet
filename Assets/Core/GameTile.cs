public class GameTile
{
    public enum Type
    {
        None,
        Ground,
        Path,
        Grass,
        Background,
        RopeMiddle,
        Treasure,
        Steps,
    }
    private Type type;
    public bool walkable;
    public bool occupied;
    public bool mesh;
    private Grid<GameTile> grid;
    public int x;
    public int y;
    public int gCost;
    public int fCost;
    public int hCost;
    public GameTile previousNode;

    public GameTile(Grid<GameTile> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        walkable = false;
        occupied = false;
    }


    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void SetIsWalkable(bool walkable)
    {
        this.walkable = walkable;
        grid.TriggerCellObjectChanged(x, y);

        if (LevelManager.Instance.tilemap != null)
        {
            LevelManager.Instance.tilemap.walkableTiles.Add(this);
        }
    }

    public override string ToString()
    {
        return System.Convert.ToInt32(walkable).ToString();
    }

    public Type GetTileType()
    {
        return type;
    }

    public void SetTileType(Type tileType)
    {
        this.type = tileType;
        grid.TriggerCellObjectChanged(x, y);
    }


    [System.Serializable]
    public class SaveObject
    {
        public Type tilemapSprite;
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
            tilemapSprite = type,
            x = x,
            y = y,
        };
    }

    public void Load(SaveObject saveObject)
    {
        type = saveObject.tilemapSprite;
    }

}
