using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Grid<TCell>
{
    public event EventHandler<OnGridChangedEventArgs> OnGridChanged;
    public class OnGridChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int columns;
    private int rows;
    private float cellSize;
    private Vector3 origin;
    private TCell[,] grid;
    private TextMesh[,] debugText;

    public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<TCell>, int, int, TCell> createCellObject)
    {
        this.columns = width;
        this.rows = height;
        this.origin = origin;
        this.cellSize = cellSize;

        grid = new TCell[width, height];
        debugText = new TextMesh[width, height];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = createCellObject(this, x, y);
            }
        }

        bool showDebug = true;

        if (showDebug)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    debugText[x, y] = UtilsClass.CreateWorldText(grid[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 40, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            OnGridChanged += (object sender, OnGridChangedEventArgs eventArgs) =>
            {
                debugText[eventArgs.x, eventArgs.y].text = grid[eventArgs.x, eventArgs.y]?.ToString();
            };
        }

    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    public void GetCell(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);

    }

    public int GetWidth()
    {
        return columns;
    }
    public int GetHeight()
    {
        return rows;
    }
    public float GetCellSize()
    {
        return cellSize;
    }

    public void SetCellObject(int x, int y, TCell value)
    {
        if (x >= 0 && y >= 0 && x < columns && y < rows)
        {
            grid[x, y] = value;
            debugText[x, y].text = grid[x, y].ToString();
            if (OnGridChanged != null) OnGridChanged(this, new OnGridChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerCellObjectChanged(int x, int y)
    {
        if (OnGridChanged != null) OnGridChanged(this, new OnGridChangedEventArgs { x = x, y = y });
    }

    public void SetCellObject(Vector3 worldPosition, TCell value)
    {
        int x, y;
        GetCell(worldPosition, out x, out y);
        SetCellObject(x, y, value);
    }

    public TCell GetCellObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < columns && y < rows)
        {
            return grid[x, y];
        }
        else
        {
            return default(TCell);
        }

    }
    public TCell GetCellObject(Vector3 worldPosition)
    {
        int x, y;
        GetCell(worldPosition, out x, out y);
        return GetCellObject(x, y);
    }

}
