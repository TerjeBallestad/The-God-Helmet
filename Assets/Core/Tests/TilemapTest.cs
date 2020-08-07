using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class TilemapTest : MonoBehaviour
{

    [SerializeField] private TilemapVisual tilemapVisual;
    private Tilemap tilemap;
    private Tilemap.Tile.Sprite tilemapSprite;

    private void Start()
    {
        tilemap = new Tilemap(20, 10, 1f, Vector3.zero);

        tilemap.SetTilemapVisual(tilemapVisual);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            tilemap.SetTilemapSprite(mouseWorldPosition, tilemapSprite);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tilemapSprite = Tilemap.Tile.Sprite.None;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            tilemapSprite = Tilemap.Tile.Sprite.Ground;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            tilemapSprite = Tilemap.Tile.Sprite.Path;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            tilemapSprite = Tilemap.Tile.Sprite.Grass;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            tilemapSprite = Tilemap.Tile.Sprite.Background;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            tilemap.Save();
            CMDebug.TextPopupMouse("Saved!");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            tilemap.Load();
            CMDebug.TextPopupMouse("Loaded!");
        }
    }

}
