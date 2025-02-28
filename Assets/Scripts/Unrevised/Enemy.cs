using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public string type;
    public Global.Element status;

    [Tooltip("NavMesh Waypoints")]
    public Waypoint[] waypoints = { };

    private NavMeshAgent _agent;
    private int _currentWaypoint = 0;

    private EnemyStats _stats;

    private Coroutine _slowCoroutine;
    private float _totalSlowDuration;
    private Coroutine _statusCoroutine;
    private float _totalStatusDuration;

    private VFXManager _vfxManager;
    private GameObject _vfxRoot;

    private float _lastReactionTime;

    private void Start()
    {
        // Setup stats
        _stats = new(Global.enemyValues[type]);

        // Setup NavMeshAgent
        _agent = GetComponent<NavMeshAgent>();

        _vfxManager = FindObjectOfType<VFXManager>();
        _vfxRoot = transform.Find("VFXroot").gameObject;

        if (_agent == null)
            Debug.LogError("Null agent");
        if (waypoints.Length == 0)
            Debug.LogError("Waypoints must not be an empty Array!");

        _agent.speed = _stats.speed;

        _lastReactionTime = -Global.reactionCooldown;
    }

    private void Update()
    {
        // Follow NavMesh route
        FollowRoute();
    }

    public void TakeDamage(float damage, Global.Element element, int towerBuffCode = Global.BUFF_NONE)
    {
        HandleReaction(element, towerBuffCode);
        HandleDamage(damage);
    }

    private void HandleReaction(Global.Element element, int towerBuffCode)
    {
        HandleTowerBuffs(towerBuffCode);

        if (status == element || (status == Global.Element.None && element != Global.Element.None))
        {
            // Reapply status or apply it if it had none.
            ApplyStatus(element);
            return;
        }

        if (status != Global.Element.None && element == Global.Element.None)
            return; // Nothing to do here

        if (Time.time - _lastReactionTime <= Global.reactionCooldown)
            return; // internal cooldown not passed yet

        // Status + Element Handler
        HandleDamage(Global.reactionValues[status][element].damage);

        // corner case for NW
        if (!(Global.reactionValues[status][element].displayName == "Terrus Aquas"))
            ApplySlow(
                Global.reactionValues[status][element].slowValue,
                Global.reactionValues[status][element].slowDuration);

        _lastReactionTime = Time.time;

        switch (Global.reactionValues[status][element].displayName)
        {
            case "Pyrus Voltes":
                // TODO
                break;
            case "Terrus Pyrus":
                // TODO
                break;
            case "Pyrus Noxius":
                // TODO
                break;
            case "Pyrus Aquas":
                // Nothing
                _vfxManager.PlayFW(_vfxRoot);
                break;
            case "Terrus Voltes":
                // TODO
                break;
            case "Noxius Voltes":
                // TODO
                break;
            case "Aquas Voltes":
                Enemy[] enemies = FindObjectsOfType<Enemy>();

                // VALUE SHOULD BE MODIFIED FOR BALANCING !!!
                float stunRadius = 3;
                foreach (Enemy enemy in enemies)
                {

                    if (Vector3.Distance(enemy.transform.position, transform.position) <= stunRadius)
                        enemy.ApplySlow(
                            Global.reactionValues[Global.Element.Lightning][Global.Element.Water].slowValue,
                            Global.reactionValues[Global.Element.Lightning][Global.Element.Water].slowDuration);
                }

                _vfxManager.PlayLW(_vfxRoot);
                break;
            case "Terrus Aquas":
                Tower[] towers = FindObjectsOfType<Tower>();

                float bestDistance = 100000;
                int bestIndex = 0;

                for (int i = 0; i < towers.Length; ++i)
                {
                    float distanceToCurrentTower = Vector3.Distance(transform.position, towers[i].transform.position);

                    if (distanceToCurrentTower < bestDistance)
                    {
                        bestDistance = distanceToCurrentTower;
                        bestIndex = i;
                    }
                }

                towers[bestIndex].Buff(Global.BUFF_SLOW);

                _vfxManager.PlayNW(_vfxRoot);
                break;
            case "Aquas Noxius":
                // TODO
                break;
            default:
                Debug.LogError("Undefined reaction!");
                print(status + " + " + element);
                break;
        }
    }

    private void HandleTowerBuffs(int towerBuffCode)
    {
        // TODO!! pentru buff shred

        if ((towerBuffCode & Global.BUFF_SLOW) != 0)
        {
            ApplySlow(
                 Global.reactionValues[Global.Element.Nature][Global.Element.Water].slowValue,
                 Global.reactionValues[Global.Element.Nature][Global.Element.Water].slowDuration);
        }
    }

    private void HandleDamage(float damage)
    {
        //print("Dealt " + damage + " damage");
        _stats.health -= damage;

        if (_stats.health <= 0)
        {
            // TODO: death animation
            Destroy(gameObject);
        }
    }

    public void ApplySlow(float slowValue, float slowDuration)
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
        if (_statusCoroutine != null)
        {
            _totalStatusDuration = Global.inflictStatusDuration;
            yield break;
        }

        status = element;
        _totalStatusDuration = Global.inflictStatusDuration;
        _statusCoroutine = StartCoroutine(StatusTimerCoroutine());

        yield return _statusCoroutine;

        status = Global.Element.None;
        _statusCoroutine = null;
    }

    private IEnumerator StatusTimerCoroutine()
    {
        float startTime = Time.time;

        while (Time.time - startTime < _totalStatusDuration)
        {
            yield return null;
        }

        if (_totalStatusDuration > Global.inflictStatusDuration)
        {
            _totalStatusDuration -= Global.inflictStatusDuration;
            _statusCoroutine = StartCoroutine(StatusTimerCoroutine());
        }
    }


    private IEnumerator ApplySlowRoutine(float slowValue, float slowDuration)
    {
        if (_slowCoroutine != null)
        {
            _totalSlowDuration = slowDuration;
            yield break;
        }

        _totalSlowDuration = slowDuration;
        _agent.speed = _stats.speed * (1 - slowValue);
        _slowCoroutine = StartCoroutine(SlowTimerCoroutine(slowDuration));

        yield return _slowCoroutine;

        _agent.speed = _stats.speed;
        _slowCoroutine = null;
    }

    private IEnumerator SlowTimerCoroutine(float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            yield return null;
        }

        // If another slow effect was added during this time, continue the effect
        if (_totalSlowDuration > duration)
        {
            _totalSlowDuration -= duration;
            _slowCoroutine = StartCoroutine(SlowTimerCoroutine(_totalSlowDuration));
        }
    }

    private void FollowRoute()
    {
        _agent.SetDestination(waypoints[_currentWaypoint].transform.position);

        float distance = Vector3.Distance(waypoints[_currentWaypoint].transform.position, transform.position);

        if (distance < 0.7)
        {
            if (_currentWaypoint >= waypoints.Length - 1)
            {
                _currentWaypoint = -1;
            }

            _currentWaypoint++;
        }
    }
}
