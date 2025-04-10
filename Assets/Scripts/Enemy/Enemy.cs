using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Global.EnemyType type;
    public Global.Element status;

    [Tooltip("NavMesh Waypoints")]
    public List<Waypoint> waypoints = new();

    [HideInInspector]
    public EnemyStats stats;

    //

    private Coroutine slowCoroutine;
    private float totalSlowDuration;
    private Coroutine statusCoroutine;
    private float totalStatusDuration;

    private VFXManager vfxManager;
    private GameObject vfxRoot;

    private float lastReactionTime;

    private NavMeshAgent agent;
    private int currentWaypoint = 0;

    private Health health;

    private void Start()
    {
        stats = new(Global.enemyValues[type]);

        health = GetComponent<Health>();
        health.maxHealth = stats.health;
        health.currentHealth = health.maxHealth;

        vfxManager = FindFirstObjectByType<VFXManager>();
        vfxRoot = transform.Find("VFXroot").gameObject;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.speed;

        if (vfxRoot == null)
            Debug.LogError(name + " has no VFXroot!");

        lastReactionTime = -Global.REACTION_COOLDOWN;



        if (agent == null)
            Debug.LogError($"{gameObject.name} is missing NavMeshAgent!");

        if (waypoints == null || waypoints.Count == 0)
            Debug.LogError($"{gameObject.name} has no waypoints set!");
    }
    

    private void Update()
    {
        FollowRoute();
    }

    public void TakeDamage(float damage, Global.Element element)
    {
        HandleElement(element);
        HandleDamage(damage);
    }

    private void HandleDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    #region Walk
    private void FollowRoute()
    {
        if (waypoints.Count == 0)
            return;

        //print(currentWaypoint);
        agent.SetDestination(waypoints[currentWaypoint].transform.position);

        float distance = Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position);

        if (distance < 0.7)
        {
            if (currentWaypoint >= waypoints.Count - 1)
            {
                currentWaypoint = -1;
            }

            currentWaypoint++;
        }
    }
    #endregion

    #region ReactionHandler
    private void HandleElement(Global.Element element)
    {
        if (status == element || (status == Global.Element.None && element != Global.Element.None))
        {
            // Reapply status or apply it if it had none.
            ApplyStatus(element);
            return;
        }

        if (status != Global.Element.None && element == Global.Element.None)
            return; // Nothing to do here

        if (Time.time - lastReactionTime <= Global.REACTION_COOLDOWN)
            return; // internal cooldown not passed yet

        // Status + Element Handler
        HandleDamage(Global.reactionValues[status][element].damage);
        ApplySlow(
            Global.reactionValues[status][element].slowValue,
            Global.reactionValues[status][element].slowDuration);
        HandleReaction(element);

        lastReactionTime = Time.time;
    }

    private void HandleReaction(Global.Element element)
    {
        if (Global.reactionValues[status][element].displayName == "Pyrus Voltes")
        {
            // TODO
            return;
        }

        if (Global.reactionValues[status][element].displayName == "Pyrus Aquas")
        {
            vfxManager.PlayFW(vfxRoot);
            return;
        }

        if (Global.reactionValues[status][element].displayName == "Aquas Voltes")
        {
            vfxManager.PlayLW(vfxRoot);

            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            // VALUE SHOULD BE MODIFIED FOR BALANCING !!!
            float stunRadius = 3;
            foreach (Enemy enemy in enemies)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) <= stunRadius)
                    enemy.ApplySlow(
                        Global.reactionValues[Global.Element.Lightning][Global.Element.Water].slowValue,
                        Global.reactionValues[Global.Element.Lightning][Global.Element.Water].slowDuration);
            }
            return;
        }
        
        Debug.LogError("Undefined reaction!");
        print(status + " + " + element);
    }

    #endregion

    #region StatusAndSlow
    private void ApplySlow(float slowValue, float slowDuration)
    {
        // IT WORKS, DON'T ASK HOW
        StartCoroutine(ApplySlowRoutine(slowValue, slowDuration));
    }

    private void ApplyStatus(Global.Element element)
    {
        // IT WORKS, DON'T ASK HOW
        StartCoroutine(ApplyStatusRoutine(element));
    }

    private IEnumerator ApplyStatusRoutine(Global.Element element)
    {
        // If another status effect is already active, update the total duration and exit
        if (statusCoroutine != null)
        {
            totalStatusDuration = Global.INFLICT_STATUS_DURATION;
            yield break;
        }

        status = element;
        totalStatusDuration = Global.INFLICT_STATUS_DURATION;
        statusCoroutine = StartCoroutine(StatusTimerCoroutine());

        yield return statusCoroutine;

        status = Global.Element.None;
        statusCoroutine = null;
    }

    private IEnumerator StatusTimerCoroutine()
    {
        float startTime = Time.time;

        while (Time.time - startTime < totalStatusDuration)
        {
            yield return null;
        }

        if (totalStatusDuration > Global.INFLICT_STATUS_DURATION)
        {
            totalStatusDuration -= Global.INFLICT_STATUS_DURATION;
            statusCoroutine = StartCoroutine(StatusTimerCoroutine());
        }
    }

    private IEnumerator ApplySlowRoutine(float slowValue, float slowDuration)
    {
        if (slowCoroutine != null)
        {
            totalSlowDuration = slowDuration;
            yield break;
        }

        totalSlowDuration = slowDuration;
        agent.speed = stats.speed * (1 - slowValue);
        slowCoroutine = StartCoroutine(SlowTimerCoroutine(slowDuration));

        yield return slowCoroutine;

        agent.speed = stats.speed;
        slowCoroutine = null;
    }

    private IEnumerator SlowTimerCoroutine(float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            yield return null;
        }

        // If another slow effect was added during this time, continue the effect
        if (totalSlowDuration > duration)
        {
            totalSlowDuration -= duration;
            slowCoroutine = StartCoroutine(SlowTimerCoroutine(totalSlowDuration));
        }
    }
    #endregion    
}
