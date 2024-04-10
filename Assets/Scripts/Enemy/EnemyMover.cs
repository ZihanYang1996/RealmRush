using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> waypoints = new List<Waypoint>();
    [SerializeField] [Range(0.1f, 20f)] float moveSpeed = 1f;
    IObjectPool<GameObject> enemyPool;

    void Awake()
    {
        // Get enemy pool
        enemyPool = GameObject.Find("Enemy Pool")?.GetComponent<EnemyPool>().ObjectPool;
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
        GameObject path = GameObject.Find("Path");
        foreach (Transform waypoint in path.transform)
        {
            waypoints.Add(waypoint.gameObject.GetComponent<Waypoint>());
        }
    }

    void ReturnToStart()
    {
        transform.position = waypoints[0].transform.position;
    }

    IEnumerator FollowPath(List<Waypoint> waypoints)
    {
        foreach (Waypoint waypoint in waypoints)
        {
            transform.LookAt(waypoint.transform.position);
            while (Vector3.Distance(transform.position, waypoint.transform.position) > Mathf.Epsilon)
            {
                Move(waypoint.transform.position, moveSpeed);
                yield return new WaitForEndOfFrame();
            }
        }
        enemyPool?.Release(gameObject);
    }

    void Move(Vector3 position, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
    }
}
