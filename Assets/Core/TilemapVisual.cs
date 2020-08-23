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
        public GameTile.Sprite tilemapSprite;
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
    private Mesh mesh;
    private bool updateMesh;
    private BoxCollider2D box;
    private Dictionary<GameTile.Sprite, UVCoords> uvCoordsDictionary;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordsDictionary = new Dictionary<GameTile.Sprite, UVCoords>();

        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUVArray)
        {
            uvCoordsDictionary[tilemapSpriteUV.tilemapSprite] = new UVCoords
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
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1);

                GameTile gridObject = grid.GetCellObject(x, y);
                GameTile.Sprite tilemapSprite = gridObject.GetTilemapSprite();
                Vector2 gridUV00, gridUV11;
                if (tilemapSprite == GameTile.Sprite.None)
                {
                    gridUV00 = Vector2.zero;
                    gridUV11 = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else
                {
                    UVCoords uvCoords = uvCoordsDictionary[tilemapSprite];
                    gridUV00 = uvCoords.uv00;
                    gridUV11 = uvCoords.uv11;
                }
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridUV00, gridUV11);
            }
        }
        // gameObject.TryGetComponent<BoxCollider2D>(out box);
        // if (box == null)
        // {
        //     box = gameObject.AddComponent<BoxCollider2D>();
        // }
        // box.size = new Vector2(grid.GetWidth() * grid.GetCellSize(), grid.GetHeight() * grid.GetCellSize());

        // box.offset = new Vector2(grid.GetWorldPosition(Mathf.RoundToInt(grid.GetWidth() / 2), Mathf.RoundToInt(grid.GetHeight() / 2)).x, grid.GetWorldPosition(Mathf.RoundToInt(grid.GetWidth() / 2), Mathf.RoundToInt(grid.GetHeight() / 2)).y + grid.GetCellSize() / 2);
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}

