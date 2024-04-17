using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid;

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        grid = gridManager?.Grid;

        startNode = grid[startCoordinates];
        destinationNode = grid[destinationCoordinates];
        BreadthFirstSearch();
    }


    void ExploreNeighbors(Node currentNode)
    {

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = currentNode.coordinates + direction;
            if (grid.ContainsKey(neighborCoordinates))
            {
                Node neighborNode = grid[neighborCoordinates];
                if (neighborNode.isWalkable && !neighborNode.isExplored)
                {
                    frontier.Enqueue(neighborNode);
                    neighborNode.isExplored = true;
                    neighborNode.comeFrom = currentNode;
                }
            }
        }
    }

    void BreadthFirstSearch()
    {
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
            ExploreNeighbors(currentSearchNode);
        }

        if (targetReached)
        {
            Node currentTracebackNode = destinationNode;
            destinationNode.isPath = true;
            while (currentTracebackNode != startNode)
            {
                Debug.Log(currentTracebackNode.coordinates);
                currentTracebackNode = currentTracebackNode.comeFrom;
                currentTracebackNode.isPath = true;
            }
        }


    }

}
