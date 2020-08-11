public class GameTile
{
    public enum Sprite
    {
        None,
        Ground,
        Path,
        Grass,
        Background,
    }
    private Sprite sprite;
    public bool walkable;
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
    }


    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void SetIsWalkable(bool isWalkable)
    {
        this.walkable = isWalkable;
        grid.TriggerCellObjectChanged(x, y);
    }

    public override string ToString()
    {
        return walkable.ToString();
    }

    public Sprite GetTilemapSprite()
    {
        return sprite;
    }

    public void SetTileSprite(Sprite tilemapSprite)
    {
        this.sprite = tilemapSprite;
        grid.TriggerCellObjectChanged(x, y);
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