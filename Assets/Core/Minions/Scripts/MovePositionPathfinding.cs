using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionPathfinding : MonoBehaviour, IMovePosition
{
    private List<Vector3> path;
    private int pathIndex = -1;

    public void SetMovePosition(Vector3 position)
    {
        path = Pathfinding.Instance.FindPath(transform.position, position);
        if (path.Count > 0)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.green, 3f);
            }
            pathIndex = 0;
        }
    }

    private void Update()
    {
        if (pathIndex != -1)
        {
            Vector3 nextPathPosition = path[pathIndex];
            Vector3 direction = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(direction);

            float reachedTargetDistance = 0.1f;
            if (Vector3.Distance(transform.position, nextPathPosition) < reachedTargetDistance)
            {
                pathIndex++;
                if (pathIndex >= path.Count)
                {
                    pathIndex = -1;
                }
            }
        }
        else
        {
            //idle
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }

}
