using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> waypoints = new List<Waypoint>();
    void Start()
    {
        StartCoroutine(FollowPath(waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FollowPath(List<Waypoint> waypoints)
    {
        foreach (Waypoint waypoint in waypoints)
        {
            transform.position = waypoint.transform.position;
            yield return new WaitForSeconds(1f);
        }
    }
}
