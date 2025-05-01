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
    private ElementalOutline outline;

    private void Start()
    {
        InitializeComponents();
        InitializeStats();
        ValidateSetup();
    }
    
    private void Update()
    {
        FollowRoute();
    }

    #region Initialization
    private void InitializeComponents()
    {
        health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        outline = GetComponent<ElementalOutline>();
        vfxManager = FindFirstObjectByType<VFXManager>();
        vfxRoot = transform.Find("VFXroot").gameObject;

        lastReactionTime = -Global.REACTION_COOLDOWN;
    }

    private void InitializeStats()
    {
        stats = new(Global.enemyValues[type]);

        health.maxHealth = stats.health;
        health.currentHealth = stats.health;

        agent.speed = stats.speed;
    }

    private void ValidateSetup()
    {
        if (vfxRoot == null)
            Debug.LogError($"{name} has no VFXroot!");

        if (agent == null)
            Debug.LogError($"{name} is missing NavMeshAgent!");

        if (waypoints == null || waypoints.Count == 0)
            Debug.LogError($"{name} has no waypoints set!");
    }
    #endregion

    #region Movement
    private void FollowRoute()
    {
        if (waypoints.Count == 0)
            return;

        agent.SetDestination(waypoints[currentWaypoint].transform.position);

        float distance = Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position);

        if (distance < 0.7f)
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
    }
    #endregion

    #region Damage Handling
    public void TakeDamage(float damage, Global.Element element)
    {
        HandleElementReaction(element);
        ApplyDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        ApplyDamage(damage);
    }

    private void ApplyDamage(float damage)
    {
        health.TakeDamage(damage);
    }
    #endregion

    #region Reaction System
    private void HandleElementReaction(Global.Element element)
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

        ReactionStats reaction = Global.reactionValues[status][element];

        // Status + Element Handler
        ApplyDamage(reaction.damage);
        ApplySlow(reaction.slowValue, reaction.slowDuration);
        ApplyReactionEffect(element);

        lastReactionTime = Time.time;
    }

    private void ApplyReactionEffect(Global.Element element)
    {
        string reactionName = Global.reactionValues[status][element].displayName;

        switch (reactionName)
        {
            case "Pyrus Voltes":
                vfxManager.PlayFL(vfxRoot);
                AffectNearbyEnemies(Global.reactionValues[Global.Element.Fire][Global.Element.Lightning], 3f);
                break;

            case "Pyrus Aquas":
                vfxManager.PlayFW(vfxRoot);
                break;

            case "Aquas Voltes":
                vfxManager.PlayLW(vfxRoot);
                AffectNearbyEnemies(Global.reactionValues[Global.Element.Water][Global.Element.Lightning], 3f);
                break;

            default:
                Debug.LogError($"Unknown reaction effect: {reactionName}");
                break;
        }
    }

    private void AffectNearbyEnemies(ReactionStats reaction, float radius)
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            if (enemy == this) continue;

            float dist = Vector3.Distance(enemy.transform.position, transform.position);

            if (dist <= radius)
            {
                enemy.TakeDamage(reaction.damage);
                enemy.ApplySlow(reaction.slowValue, reaction.slowDuration);
            }
        }
    }
    #endregion

    #region Status & Slow System
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

        if (outline != null)
            outline.SetHpBarColor(status);

        yield return statusCoroutine;

        status = Global.Element.None;
        statusCoroutine = null;

        if (outline != null)
            outline.SetHpBarColor(Global.Element.None);
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
