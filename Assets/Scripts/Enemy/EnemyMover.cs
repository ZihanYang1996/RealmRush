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
    [SerializeField] List<Tile> waypoints = new List<Tile>();
    [SerializeField] [Range(0.1f, 20f)] float moveSpeed = 1f;
    IObjectPool<GameObject> enemyPool;
    Enemy enemy;

    void Awake()
    {
        // Get enemy pool
        enemyPool = GameObject.Find("Enemy Pool")?.GetComponent<EnemyPool>().ObjectPool;
        enemy = gameObject.GetComponent<Enemy>();
        FindPath();
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

    void FindPath()
    {
        waypoints.Clear();
        GameObject path = GameObject.Find("Path");
        foreach (Transform waypoint in path.transform)
        {
            Tile wp = waypoint.gameObject.GetComponent<Tile>();
            if (wp)
            {
                waypoints.Add(wp);
            }
            
        }
    }

    void ReturnToStart()
    {
        transform.position = waypoints[0].transform.position;
    }

    void FinishPath()
    {
        enemyPool?.Release(gameObject);
        enemy.PunishGold();
    }

    IEnumerator FollowPath(List<Tile> waypoints)
    {
        foreach (Tile waypoint in waypoints)
        {
            transform.LookAt(waypoint.transform.position);
            while (Vector3.Distance(transform.position, waypoint.transform.position) > Mathf.Epsilon)
            {
                Move(waypoint.transform.position, moveSpeed);
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
