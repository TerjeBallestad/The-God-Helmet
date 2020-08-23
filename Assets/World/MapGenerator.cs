using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    public int width;
    public int height;
    public int BorderSize = 5;
    public int Iterations = 1;
    public bool showDebug;
    [Range(0, 100)]
    public int FillPercent = 50;
    int[,] map;
    Grid<GameTile> grid;
    bool baseSpawned = false;
    private void Awake()
    {
        Instance = this;
    }

    public Grid<GameTile> GenerateMap(System.Random randomNumber)
    {
        Vector3 origin = new Vector3(-width * 0.5f - BorderSize, -height - BorderSize * 2);
        grid = new Grid<GameTile>(width + BorderSize * 2, height + BorderSize * 2, origin, (Grid<GameTile> g, int x, int y) => new GameTile(g, x, y), showDebug);
        map = new int[width, height];

        RandomFillMap(randomNumber);

        for (int i = 0; i < Iterations; i++)
        {
            map = SmoothMap();
        }

        int[,] borderedMap = new int[width + BorderSize * 2, height + BorderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                if (x >= BorderSize && x < width + BorderSize && y >= BorderSize && y < height + BorderSize)
                {
                    borderedMap[x, y] = map[x - BorderSize, y - BorderSize];
                    if (borderedMap[x, y] == 0 && borderedMap[x, y - 1] == 1)
                    {
                        grid.GetCellObject(x, y).SetIsWalkable(true);
                    }
                    // grid.GetCellObject(x, y).SetIsWalkable(map[x - BorderSize, y - BorderSize] != 0);

                }
                else if (y == height + BorderSize)
                {
                    borderedMap[x, y] = 0;
                    if (borderedMap[x, y - 1] == 1)
                    {
                        grid.GetCellObject(x, y).SetIsWalkable(true);
                        if (
                            !baseSpawned &&
                            x > 3 &&
                        grid.GetCellObject(x - 1, y).walkable == true &&
                        grid.GetCellObject(x - 2, y).walkable == true &&
                        grid.GetCellObject(x - 3, y).walkable == true)
                        {
                            MinionManager.Instance.SpawnBase(grid.GetWorldPosition(x - 3, y));
                            baseSpawned = true;
                        }
                    }
                }
                else
                {
                    borderedMap[x, y] = 1;
                    // grid.GetCellObject(x, y).SetIsWalkable(true);
                }
            }
        }
        MeshGenerator.Instance.GenerateMesh(borderedMap);

        GeneratePolyCollider();

        return grid;
    }

    void RandomFillMap(System.Random randomNumber)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (randomNumber.Next(0, 100) < FillPercent) ? 1 : 0;
                }
            }
        }
    }

    int[,] SmoothMap()
    {
        int[,] tempMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 5)
                {
                    tempMap[x, y] = 1;
                }
                else if (neighbourWallTiles < 5)
                {
                    tempMap[x, y] = 0;
                }
            }
        }
        return tempMap;
    }
    int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void GeneratePolyCollider()
    {
        PolygonCollider2D poly = gameObject.AddComponent<PolygonCollider2D>();
        Vector2[] path = new Vector2[4];
        path[0] = new Vector2(grid.GetWorldPosition(0, 0).x + 0.5f, grid.GetWorldPosition(0, 0).y + 0.5f);
        path[1] = new Vector2(grid.GetWorldPosition(0, grid.GetHeight()).x + 0.5f, grid.GetWorldPosition(0, grid.GetHeight()).y + 3);
        path[2] = new Vector2(grid.GetWorldPosition(grid.GetWidth(), grid.GetHeight()).x - 0.5f, grid.GetWorldPosition(grid.GetWidth(), grid.GetHeight()).y + 3);
        path[3] = new Vector2(grid.GetWorldPosition(grid.GetWidth(), 0).x - 0.5f, grid.GetWorldPosition(grid.GetWidth(), 0).y + 0.5f);

        poly.SetPath(0, path);

        GameManager.Instance.cameraConfiner = poly;
    }
}