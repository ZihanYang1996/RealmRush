using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Vector3> waypoints = new List<Vector3>();
    [SerializeField] [Range(0.1f, 20f)] float moveSpeed = 1f;
    IObjectPool<GameObject> enemyPool;
    Enemy enemy;
    PathFinder pathFinder;
    GridManager gridManager;

    Coroutine followPathCoroutine;

    void Awake()
    {
        // Get enemy pool
        enemyPool = GameObject.Find("Enemy Pool")?.GetComponent<EnemyPool>().ObjectPool;
        enemy = gameObject.GetComponent<Enemy>();
        pathFinder = FindObjectOfType<PathFinder>();
        gridManager = FindObjectOfType<GridManager>();
    }

    void OnEnable()
    {
        EventManager.Instance.RecreatePathMessageReceived += MoveEnemy;
        MoveEnemy(initialPath: true);
    }

    void OnDisable()
    {
        EventManager.Instance.RecreatePathMessageReceived -= MoveEnemy;
        if (followPathCoroutine != null) StopCoroutine(followPathCoroutine);
    }

    void MoveEnemy(bool initialPath = true)
    {
        if (!initialPath) Debug.Log("Recreating path");  // Debug
        // Find path, wether it's the initial path or not
        FindPath(initialPath);
        // Return to start only if it's the initial path
        if (initialPath) ReturnToStart();
        // Stop the current coroutine and start a new one
        if (followPathCoroutine != null) StopCoroutine(followPathCoroutine);
        followPathCoroutine = StartCoroutine(FollowPath(waypoints));
    }

    void FindPath(bool initialPath = true)
    {
        waypoints.Clear();
        List<Node> path = new List<Node>();
        if (initialPath)
        {
            path = pathFinder.DefaultPath;
        }
        else
        {
            Vector2Int currentCoordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            path = pathFinder.BreadthFirstSearch(currentCoordinates, pathFinder.DestinationCoordinates);
        }
        
        foreach (Node node in path)
        {
            waypoints.Add(gridManager.GetPositionFromCoordinates(node.coordinates));            
        }
    }

    void ReturnToStart()
    {
        // Debug.Log("Returning to start: " + waypoints[0] + " from " + transform.position);
        transform.position = waypoints[0];
        // Debug.Log("Returned to start: " + waypoints[0] + " from " + transform.position);

    }

    void FinishPath()
    {
        enemyPool?.Release(gameObject);
        enemy.PunishGold();
    }

    IEnumerator FollowPath(List<Vector3> waypoints)
    {
        if (waypoints.Count == 0) yield break;  // No waypoints
        // Debug.Log("Following path");
        for (int i = 1; i < waypoints.Count; i++)
        {
            Vector3 waypoint = waypoints[i];
            transform.LookAt(waypoint);
            while (Vector3.Distance(transform.position, waypoint) > Mathf.Epsilon)
            {
                // Debug.Log("Moving to " + waypoint + " from " + transform.position);
                Move(waypoint, moveSpeed);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }

    void Move(Vector3 position, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
    }
}
