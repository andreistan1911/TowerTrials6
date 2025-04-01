using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoEnemy : MonoBehaviour
{
    public Global.Element status;

    [Tooltip("NavMesh Waypoints")]
    public Waypoint[] waypoints = { };

    private NavMeshAgent _agent;
    private int _currentWaypoint = 0;

    public float speed = 1.5f;

    private void Start()
    {
        // Setup NavMeshAgent
        _agent = GetComponent<NavMeshAgent>();

        _agent.speed = speed;
    }

    private void Update()
    {
        // Follow NavMesh route
        FollowRoute();
    }

    private void FollowRoute()
    {
        _agent.SetDestination(waypoints[_currentWaypoint].transform.position);

        float distance = Vector3.Distance(waypoints[_currentWaypoint].transform.position, transform.position);

        if (distance < 1.2)
        {
            if (_currentWaypoint >= waypoints.Length - 1)
            {
                _currentWaypoint = -1;
            }

            _currentWaypoint++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
            Destroy(gameObject);
    }
}
