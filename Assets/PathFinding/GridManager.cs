using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }
    [SerializeField] Vector2Int xrange;
    [SerializeField] Vector2Int yrange;

    int unityGridSize = 10;
    public int UnityGridSize { get { return unityGridSize; } }

    void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    void CreateGrid()
    {
        for (int x = xrange.x; x <= xrange.y; x++)
        {
            for (int y = yrange.x; y <= yrange.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                Node node = new Node(coordinates, true);
                grid.Add(coordinates, node);
            }
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int
        {
            x = Mathf.RoundToInt(position.x / unityGridSize),
            y = Mathf.RoundToInt(position.z / unityGridSize)
        };
        return coordinates;
    }

}
