/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapVisual : MonoBehaviour
{

    [System.Serializable]
    public struct TilemapSpriteUV
    {
        public GameTile.Type tileType;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private TilemapSpriteUV[] tilemapSpriteUVArray;
    private Grid<GameTile> grid;
    private Mesh itemMesh;
    private Mesh selectableIndicatorMesh;
    private bool updateMesh;
    private BoxCollider2D box;
    private Dictionary<GameTile.Type, UVCoords> uvCoordsDictionary;


    private void Awake()
    {
        itemMesh = new Mesh();
        selectableIndicatorMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = itemMesh;
        itemMesh.name = "items";
        GameObject selectableIndicator = new GameObject("selectable indicator");
        selectableIndicator.transform.SetParent(transform);
        selectableIndicator.AddComponent<MeshFilter>().mesh = selectableIndicatorMesh;
        selectableIndicator.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        selectableIndicatorMesh.name = "selectable";






        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordsDictionary = new Dictionary<GameTile.Type, UVCoords>();

        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUVArray)
        {
            uvCoordsDictionary[tilemapSpriteUV.tileType] = new UVCoords
            {
                uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
    }

    public void SetGrid(Tilemap tilemap, Grid<GameTile> grid)
    {
        this.grid = grid;
        UpdateTilemapVisual();

        grid.OnGridChanged += Grid_OnGridValueChanged;
        tilemap.OnLoaded += Tilemap_OnLoaded;
    }

    private void Tilemap_OnLoaded(object sender, System.EventArgs e)
    {
        updateMesh = true;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<GameTile>.OnGridChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateTilemapVisual();
        }
    }

    private void UpdateTilemapVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] itemVertices, out Vector2[] itemUV, out int[] itemTriangles);
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] selectableVerices, out Vector2[] selectableUV, out int[] selectableTriangles);


        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1);

                GameTile gridObject = grid.GetCellObject(x, y);
                GameTile.Type tileType = gridObject.GetTileType();
                Vector2 gridUV00, gridUV11;
                if (tileType == GameTile.Type.None)
                {
                    gridUV00 = Vector2.zero;
                    gridUV11 = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else
                {
                    UVCoords uvCoords = uvCoordsDictionary[tileType];
                    gridUV00 = uvCoords.uv00;
                    gridUV11 = uvCoords.uv11;
                }

                MeshUtils.AddToMeshArrays(itemVertices, itemUV, itemTriangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridUV00, gridUV11);

                if (gridObject.selectable)
                {
                    UVCoords uvCoords = uvCoordsDictionary[GameTile.Type.Selectable];
                    gridUV00 = uvCoords.uv00;
                    gridUV11 = uvCoords.uv11;
                    quadSize = new Vector3(1, 1);

                }
                MeshUtils.AddToMeshArrays(selectableVerices, selectableUV, selectableTriangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridUV00, gridUV11);



            }
        }



        selectableIndicatorMesh.vertices = selectableVerices;
        selectableIndicatorMesh.uv = selectableUV;
        selectableIndicatorMesh.triangles = selectableTriangles;


        itemMesh.vertices = itemVertices;
        itemMesh.uv = itemUV;
        itemMesh.triangles = itemTriangles;

    }

}

