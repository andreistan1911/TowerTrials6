using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Global.EnemyType type;
    public Global.Element status;

    [HideInInspector]
    public EnemyStats stats;

    private Walkable walkable;

    private Coroutine slowCoroutine;
    private float totalSlowDuration;
    private Coroutine statusCoroutine;
    private float totalStatusDuration;

    private VFXManager vfxManager;
    private GameObject vfxRoot;

    private float lastReactionTime;

    private void Start()
    {
        stats = new(Global.enemyValues[type]);
        walkable = GetComponent<Walkable>();

        vfxManager = FindFirstObjectByType<VFXManager>();
        vfxRoot = transform.Find("VFXroot").gameObject;

        if (vfxRoot == null)
            Debug.LogError(name + " has no VFXroot!");

        lastReactionTime = -Global.REACTION_COOLDOWN;
    }

    private void Update()
    {
        // NOOP
    }

    public void TakeDamage(float damage, Global.Element element)
    {
        HandleElement(element);
        HandleDamage(damage);
    }

    private void HandleDamage(float damage)
    {
        //print("Dealt " + damage + " damage");
        stats.health -= damage;

        if (stats.health <= 0)
        {
            // TODO: death animation
            Destroy(gameObject);
        }
    }

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
        walkable.agent.speed = stats.speed * (1 - slowValue);
        slowCoroutine = StartCoroutine(SlowTimerCoroutine(slowDuration));

        yield return slowCoroutine;

        walkable.agent.speed = stats.speed;
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
