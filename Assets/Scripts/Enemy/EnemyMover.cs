using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> waypoints = new List<Waypoint>();
    [SerializeField] [Range(0.1f, 20f)] float moveSpeed = 1f;
    void Start()
    {
        StartCoroutine(FollowPath(waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        // Move(new Vector3(0, 0, 20), moveSpeed);
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
    }

    void Move(Vector3 position, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
    }
}
