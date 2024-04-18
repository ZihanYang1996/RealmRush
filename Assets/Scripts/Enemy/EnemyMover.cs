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

    void Awake()
    {
        // Get enemy pool
        enemyPool = GameObject.Find("Enemy Pool")?.GetComponent<EnemyPool>().ObjectPool;
        enemy = gameObject.GetComponent<Enemy>();
        pathFinder = FindObjectOfType<PathFinder>();
        gridManager = FindObjectOfType<GridManager>();
        FindPath(initialPath: true);
    }

    void OnEnable()
    {
        ReturnToStart();
        StartCoroutine(FollowPath(waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        // Move(new Vector3(0, 0, 20), moveSpeed);
    }

    void FindPath(bool initialPath = true)
    {
        waypoints.Clear();
        List<Node> path = new List<Node>();
        if (initialPath)
        {
            path = pathFinder.BreadthFirstSearch(pathFinder.StartCoordinates, pathFinder.DestinationCoordinates);
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
        Debug.Log("Returning to start");
        transform.position = waypoints[0];
    }

    void FinishPath()
    {
        enemyPool?.Release(gameObject);
        enemy.PunishGold();
    }

    IEnumerator FollowPath(List<Vector3> waypoints)
    {
        foreach (Vector3 waypoint in waypoints)
        {
            transform.LookAt(waypoint);
            while (Vector3.Distance(transform.position, waypoint) > Mathf.Epsilon)
            {
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
