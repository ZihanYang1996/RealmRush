using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }


    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid;

    List<Node> defaultPath;
    public List<Node> DefaultPath
    {
        get
        {
            return new List<Node>(defaultPath);
        }
    }

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        grid = gridManager?.Grid;

        // transform.position = gridManager.GetPositionFromCoordinates(startCoordinates);  // Move the EnemyPool object to the start position, not necessary

        defaultPath = BreadthFirstSearch(startCoordinates, destinationCoordinates);
    }

    public bool generateDefaultPath()
    {
        List<Node> newPath = BreadthFirstSearch(startCoordinates, destinationCoordinates);
        if (newPath.Count == 0)
        {
            return false;
        }
        else
        {
            defaultPath = newPath;
            return true;
        }
        
    }


    List<Node> GetNeighbors(Node currentNode)
    {
        List<Node> neighbors = new List<Node>();
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = currentNode.coordinates + direction;
            if (grid.ContainsKey(neighborCoordinates))
            {
                Node neighborNode = grid[neighborCoordinates];
                if (neighborNode.isWalkable)
                {
                    neighbors.Add(neighborNode);
                }
            }
        }
        return neighbors;
    }

    public List<Node> BreadthFirstSearch(Vector2Int startCoordinates, Vector2Int destinationCoordinates)
    {
        Node currentSearchNode;
        Queue<Node> frontier = new Queue<Node>();
        Dictionary<Node, Node> comeFrom = new Dictionary<Node, Node>();
        List<Node> path = new List<Node>();

        Node startNode = grid[startCoordinates];
        Node destinationNode = grid[destinationCoordinates];



        bool targetReached = false;

        if (startNode.isWalkable)
        {
            frontier.Enqueue(startNode);
            startNode.isExplored = true;
        }

        while (!targetReached && frontier.Count > 0)
        {
            currentSearchNode = frontier.Dequeue();

            if (currentSearchNode == destinationNode)
            {
                targetReached = true;
            }
            foreach (Node neighbor in GetNeighbors(currentSearchNode))
            {
                if (!comeFrom.ContainsKey(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    comeFrom.Add(neighbor, currentSearchNode);
                    neighbor.isExplored = true;
                }

            }
        }

        if (targetReached)
        {
            Node currentTracebackNode = destinationNode;
            destinationNode.isPath = true;
            path.Add(currentTracebackNode);
            while (currentTracebackNode != startNode)
            {
                currentTracebackNode = comeFrom[currentTracebackNode];
                currentTracebackNode.isPath = true;
                path.Add(currentTracebackNode);
            }
            path.Reverse();
        }

        
        // foreach (Node node in path)
        // {
        //     Debug.Log(node.coordinates);
        // }
        return path;
    }
}
