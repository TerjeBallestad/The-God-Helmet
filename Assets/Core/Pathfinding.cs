using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public static Pathfinding Instance { get; private set; }
    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public Pathfinding(int width, int height)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 1f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }
    public Grid<PathNode> GetGrid()
    {
        return grid;
    }
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        grid.GetCell(startWorldPosition, out int startX, out int startY);
        grid.GetCell(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode node in path)
            {
                vectorPath.Add(new Vector3(node.x, node.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetCellObject(startX, startY);
        PathNode endNode = grid.GetCellObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetCellObject(x, y);
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
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                //Reached final node
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourNodes(currentNode))
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

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodes)
    {
        PathNode lowestCostNode = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].fCost < lowestCostNode.fCost)
            {
                lowestCostNode = nodes[i];
            }
        }
        return lowestCostNode;
    }
    private List<PathNode> GetNeighbourNodes(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            //left
            neighbours.Add(GetNode(currentNode.x - 1, currentNode.y));
            //left down
            if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            //left up
            if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y + 1));

        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            //right
            neighbours.Add(GetNode(currentNode.x + 1, currentNode.y));
            //right down
            if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // right up
            if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        //down
        if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x, currentNode.y - 1));
        //up
        if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbours;
    }
    public PathNode GetNode(int x, int y)
    {
        return grid.GetCellObject(x, y);
    }
}

public class PathNode
{
    public bool walkable;
    private Grid<PathNode> grid;
    public int x;
    public int y;
    public int gCost;
    public int fCost;
    public int hCost;
    public PathNode previousNode;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        walkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void SetIsWalkable(bool isWalkable)
    {
        this.walkable = isWalkable;
        grid.TriggerCellObjectChanged(x, y);
    }

    public override string ToString()
    {
        return x + "," + y;
    }

}
