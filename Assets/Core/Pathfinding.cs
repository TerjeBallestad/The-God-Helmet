using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public static Pathfinding Instance { get; private set; }
    private Grid<GameTile> grid;
    private List<GameTile> openList;
    private List<GameTile> closedList;
    public Pathfinding(Grid<GameTile> grid)
    {
        Instance = this;
        this.grid = grid;
    }
    public Grid<GameTile> GetGrid()
    {
        return grid;
    }
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        grid.GetCell(startWorldPosition, out int startX, out int startY);
        grid.GetCell(endWorldPosition, out int endX, out int endY);

        List<GameTile> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (GameTile node in path)
            {
                vectorPath.Add(new Vector3(node.x + (grid.GetCellSize() * 0.5f), node.y + grid.GetCellSize()));
            }
            return vectorPath;
        }
    }
    public List<GameTile> FindPath(int startX, int startY, int endX, int endY)
    {
        GameTile startNode = grid.GetCellObject(startX, startY);
        GameTile endNode = grid.GetCellObject(endX, endY);

        openList = new List<GameTile> { startNode };
        closedList = new List<GameTile>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                GameTile pathNode = grid.GetCellObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.previousNode = null;
            }

        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            GameTile currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                //Reached final node
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GameTile neighbourNode in GetNeighbourNodes(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.walkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        // Out of nodes on the openList
        return null;
    }

    private List<GameTile> CalculatePath(GameTile endNode)
    {
        List<GameTile> path = new List<GameTile>();
        path.Add(endNode);
        GameTile currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(GameTile a, GameTile b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private GameTile GetLowestFCostNode(List<GameTile> nodes)
    {
        GameTile lowestCostNode = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].fCost < lowestCostNode.fCost)
            {
                lowestCostNode = nodes[i];
            }
        }
        return lowestCostNode;
    }
    private List<GameTile> GetNeighbourNodes(GameTile currentNode)
    {
        List<GameTile> neighbours = new List<GameTile>();

        if (currentNode.x - 1 >= 0)
        {
            //left
            neighbours.Add(GetNode(currentNode.x - 1, currentNode.y));
            //left down
            // if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            //left up
            // if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y + 1));

        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            //right
            neighbours.Add(GetNode(currentNode.x + 1, currentNode.y));
            //right down
            // if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // right up
            // if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        //down
        if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x, currentNode.y - 1));
        //up
        if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbours;
    }
    public GameTile GetNode(int x, int y)
    {
        return grid.GetCellObject(x, y);
    }
}
