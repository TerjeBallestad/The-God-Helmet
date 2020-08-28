using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    public int width;
    public int height;
    public int BorderSize = 5;
    public int Iterations = 1;
    public int RoomThreshold = 5;
    public bool showDebug;
    [Range(0, 100)]
    public int FillPercent = 50;
    public int[,] map;
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
        ProcessMap();
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
        map = borderedMap;

        GeneratePolyCollider();

        return grid;
    }

    public bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void ProcessMap()
    {
        List<List<Coord>> roomRegions = GetRegions(0);
        List<Room> roomsAboveThreshold = new List<Room>();
        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < RoomThreshold)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.x, tile.y] = 1;
                }
            }
            else roomsAboveThreshold.Add(new Room(roomRegion, map));
        }
        roomsAboveThreshold.Sort();
        foreach (Room r in roomsAboveThreshold)
        {
            Debug.Log(r.roomSize);
        }
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);
                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.x, tile.y] = 1;
                    }
                }
            }
        }
        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;
        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);
            for (int x = tile.x - 1; x <= tile.x + 1; x++)
            {
                for (int y = tile.y - 1; y <= tile.y + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.y || x == tile.x))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }

            }
        }
        return tiles;
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
                if (IsInMapRange(neighbourX, neighbourY))
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
    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public int roomSize;
        public Room()
        {

        }
        public Room(List<Coord> tiles, int[,] map)
        {
            this.tiles = tiles;
            roomSize = tiles.Count;

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles)
            {
                for (int x = tile.x - 1; x <= tile.x + 1; x++)
                {
                    for (int y = tile.y - 1; y <= tile.y + 1; y++)
                    {
                        if (x == tile.x || y == tile.y)
                        {
                            if (x > 0 && x < map.GetLength(0) && y > 0 && y < map.GetLength(1))
                            {
                                if (map[x, y] == 1)
                                {
                                    edgeTiles.Add(tile);
                                }
                            }
                        }
                    }
                }
            }
        }
        public int CompareTo(Room other)
        {
            return other.roomSize.CompareTo(roomSize);
        }
    }

    struct Coord
    {
        public int x;
        public int y;
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}