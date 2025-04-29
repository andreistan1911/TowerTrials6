using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Tower tower;
    private Dictionary<Enemy, float> enemyTimers; // <enemy object, time spent in range>

    private void Start()
    {
        tower = transform.parent.GetComponent<Tower>();
        enemyTimers = new Dictionary<Enemy, float>();

        // Just to be sure
        if (tower == null)
        {
            Debug.Log("Error: tower not found from range");
            return;
        }

        tower.range = this;
    }

    private void Update()
    {
        if (enemyTimers.Count == 0)
            tower.NoEnemyInRange();
        else
            tower.EnemyIsInRange();

        foreach (var enemy in enemyTimers.Keys.ToList())
        {
            if (enemy == null)
            {
                enemyTimers.Remove(enemy);
                continue;
            }

            enemyTimers[enemy] += Time.deltaTime;

            if (enemyTimers[enemy] >= tower.attackRate)
            {
                enemyTimers[enemy] = 0;
                tower.Fire(enemy);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy == null)
            return;
        
        enemyTimers.Add(enemy, 0);
        tower.Fire(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy == null)
            return;

        enemyTimers[enemy] = 0;
        enemyTimers.Remove(enemy);
    }
}
