using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractWaveManger : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject waypointsCollection;
    public float timeBetweenSpawns = 1f;

    protected readonly List<Waypoint> waypoints = new();

    public static Dictionary<Global.EnemyType, int> baseCosts = new()
    {
        { Global.EnemyType.Slime, 1 },
        { Global.EnemyType.Wolf, 2 },
        { Global.EnemyType.Goblin, 3 },
        { Global.EnemyType.Dragon, 4 },
        { Global.EnemyType.Skeleton, 5 },
        { Global.EnemyType.Viking, 6 },
        { Global.EnemyType.Demon, 7 },
        { Global.EnemyType.Giant, 8 }
    };

    private void Start()
    {
        // Clear existing, just in case
        waypoints.Clear();

        foreach (Transform child in waypointsCollection.transform)
        {
            Waypoint wp = child.GetComponent<Waypoint>();
            if (wp != null)
            {
                waypoints.Add(wp);
            }
        }
    }

    private void Update()
    {
        
    }

    public abstract void Spawn(int currentWave);

    protected IEnumerator SpawnWave(List<EnemySpawnData> enemies, bool isLastWave = false)
    {
        WaveStateManager.Instance.OnWaveStarted();

        foreach (EnemySpawnData enemyData in enemies)
        {
            GameObject enemyPrefab = Resources.Load<GameObject>
                ($"Prefabs/Enemies/{baseCosts[enemyData.type]}_{enemyData.type}/{enemyData.element}_{enemyData.type}");

            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Enemy enemy = enemyInstance.GetComponent<Enemy>();

            enemy.waypoints = waypoints;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        if (isLastWave)
        {
            StartCoroutine(WaitUntilNoEnemiesAndCallEnd());
        }
    }

    private IEnumerator WaitUntilNoEnemiesAndCallEnd()
    {
        while (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length > 0)
            yield return null;

        WaveStateManager.Instance.OnLastWaveEnded();
    }
}
