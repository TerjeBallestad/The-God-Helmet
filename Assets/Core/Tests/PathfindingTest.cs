using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class PathfindingTest : MonoBehaviour
{
    private Pathfinding pathfinding;
    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(100, 100);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetCell(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i = 1; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) + Vector3.one * 0.5f, new Vector3(path[i + 1].x, path[i + 1].y) + Vector3.one * 0.5f, Color.green, 2f);
                }
                Debug.Log(path);
            }
        }
    }
}