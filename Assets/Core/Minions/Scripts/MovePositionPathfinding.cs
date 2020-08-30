using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MovePositionPathfinding : MonoBehaviour, IMovePosition
{
    private List<Vector3> path;
    private int pathIndex = -1;
    bool atDestination;
    Minion minion;
    float reachedTargetDistance = 0.1f;

    private void Start()
    {
        minion = GetComponent<Minion>();
    }

    public void SetMovePosition(Vector3 position)
    {
        Vector3 startPosition = LevelManager.Instance.tilemap.grid.GetWorldPosition(LevelManager.Instance.currentTile.x, LevelManager.Instance.currentTile.y);
        List<Vector3> tempPath = Pathfinding.Instance.FindPath(startPosition, position);
        if (tempPath != null && tempPath.Count > 1)
        {
            path = tempPath;

            // for (int i = 0; i < path.Count - 1; i++)
            // {
            //     Debug.DrawLine(path[i], path[i + 1], Color.green, 3f);
            // }
            pathIndex = 0;
            LevelManager.Instance.tilemap.ClearSelectableTiles();
            // if (minion) minion.RemoveSteps(path.Count);
        }

    }
    public bool AtDestination()
    {
        Debug.Log(atDestination);
        return atDestination;
    }

    private void Update()
    {
        if (pathIndex != -1)
        {
            if (minion) { minion.DeActivate(); }

            Vector3 nextPathPosition = path[pathIndex];
            if (Vector3.Distance(transform.position, nextPathPosition) < reachedTargetDistance)
            {
                pathIndex++;
                if (pathIndex >= path.Count)
                {
                    pathIndex = -1;
                    LevelManager.Instance.currentTile = LevelManager.Instance.tilemap.grid.GetCellObject(nextPathPosition);
                    if (minion) { minion.Activate(); }

                }
            }
            Vector3 direction = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(direction);

        }
        else
        {
            //idle
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
            // if (minion) minion.Activate();

        }
    }

}
