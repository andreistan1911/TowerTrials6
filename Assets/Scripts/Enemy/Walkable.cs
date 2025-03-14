using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class Walkable : MonoBehaviour
{
    [Tooltip("NavMesh Waypoints")]
    public Waypoint[] waypoints = { };

    [HideInInspector]
    public NavMeshAgent agent;

    private Enemy enemy;
    private int currentWaypoint = 0;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        // Setup NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemy.stats.speed;

        if (agent == null)
            Debug.LogError("Null agent");
        if (waypoints.Length == 0)
            Debug.LogError("Waypoints must not be an empty Array!");
    }

    private void Update()
    {
        FollowRoute();
    }

    private void FollowRoute()
    {
        if (waypoints.Length == 0)
            return;

        print(currentWaypoint);
        agent.SetDestination(waypoints[currentWaypoint].transform.position);

        float distance = Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position);

        if (distance < 0.7)
        {
            if (currentWaypoint >= waypoints.Length - 1)
            {
                currentWaypoint = -1;
            }

            currentWaypoint++;
        }
    }

}
