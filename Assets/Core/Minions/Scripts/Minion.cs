using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public MinionData data;
    private SpriteRenderer spriteRenderer;
    public int steps;
    private IMovePosition movement;
    public bool active;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.Sprite;
        name = data.name;
        HPandDMGUI HPUI = GetComponentInChildren<HPandDMGUI>();
        HPUI.UpdateHPandDMG(data.Health, data.Damage);
        HPUI.transform.localPosition = data.HPandDMGposition;
        movement = GetComponent<IMovePosition>();
        steps = data.Steps;
    }

    public void RemoveSteps(int amount)
    {
        steps -= amount;
    }

    public void NewTurn()
    {
        steps = data.Steps;
    }
    public void Activate()
    {
        active = true;
        GameTile startTile = LevelManager.Instance.tilemap.grid.GetCellObject(transform.position);
        int startX = startTile.x;
        int startY = startTile.y;
        List<GameTile> path;

        for (int x = startX - steps; x < startX + steps; x++)
        {
            for (int y = startY - steps; y < startY + steps; y++)
            {
                GameTile endTile = LevelManager.Instance.tilemap.grid.GetCellObject(x, y);
                if (endTile != null && endTile.walkable == true)
                {
                    path = Pathfinding.Instance.FindPath(startX, startY, endTile.x, endTile.y);
                    if (path == null || path.Count < 1) break;
                    foreach (GameTile tile in path)
                    {
                        tile.SetTileType(GameTile.Type.Steps);

                    }
                }

            }
        }
    }
    public void DeActivate()
    {
        active = false;
    }
    public void StartGoingToDestination(Vector3 destination)
    {
        if (active)
            movement.SetMovePosition(destination);
    }
}
