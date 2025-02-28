using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeWeaponHitbox : MonoBehaviour
{
    private float _dpt; // damage per tick
    private Global.Element _element;

    private Dictionary<Enemy, float> _dpsTimers;

    private void Start()
    {
        _dpt = transform.parent.GetComponent<ConeWeapon>().dps / Global.dpsTicks;
        _element = transform.parent.GetComponent<ConeWeapon>().element;


        _dpsTimers = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        // I can shoot just enemies
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        Enemy enemy = other.GetComponent<Enemy>();

        if (!_dpsTimers.ContainsKey(enemy))
            _dpsTimers.Add(enemy, 0);

        enemy.TakeDamage(_dpt, _element); // TODO: glitch: you can flick around for more dps, but not a priority now
    }

    private void OnTriggerStay(Collider other)
    {
        // I can shoot just enemies
        if (!other.gameObject.CompareTag("Enemy"))
            return;
        
        Enemy enemy = other.GetComponent<Enemy>();

        _dpsTimers[enemy] += Time.deltaTime;

        if (_dpsTimers[enemy] >= Global.dpsCooldown)
        {
            _dpsTimers[enemy] = 0;
            enemy.TakeDamage(_dpt, _element);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // I can shoot just enemies
        if (!other.gameObject.CompareTag("Enemy"))
            return;

        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        _dpsTimers[enemy] = 0;
        _dpsTimers.Remove(enemy);
    }
}
