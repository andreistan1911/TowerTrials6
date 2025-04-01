using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeWeaponHitbox : MonoBehaviour
{
    private float dpt; // damage per tick
    private Global.Element element;

    private Dictionary<Enemy, float> dpsTimers;

    private void Start()
    {
        dpt = transform.parent.GetComponent<ConeWeapon>().dps / Global.DPS_TICKS;
        element = transform.parent.GetComponent<ConeWeapon>().element;

        dpsTimers = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy == null)
            return;

        if (!dpsTimers.ContainsKey(enemy))
            dpsTimers.Add(enemy, 0);

        enemy.TakeDamage(dpt, element); // TODO: glitch: you can flick around for more dps, but not a priority now
    }

    private void OnTriggerStay(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy == null)
            return;

        if (!dpsTimers.ContainsKey(enemy))
            return;

            dpsTimers[enemy] += Time.deltaTime;

        if (dpsTimers[enemy] >= Global.dpsCooldown)
        {
            dpsTimers[enemy] = 0;
            enemy.TakeDamage(dpt, element);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy == null)
            return;

        dpsTimers[enemy] = 0;
        dpsTimers.Remove(enemy);
    }
}
