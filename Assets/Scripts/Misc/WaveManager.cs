using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public Global.EnemyType type;
    public Global.Element element;

    public bool HasElement()
    {
        if (element == Global.Element.None)
            return false;

        return true;
    }

    public int GetCost()
    {
        return WaveManager.baseCosts[type] + (HasElement() ? 1 : 0);
    }
}

public class WaveManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject waypointsCollection;
    public float timeBetweenSpawns = 1f;

    private List<Waypoint> waypoints = new();

    public static Dictionary<Global.EnemyType, int> baseCosts = new()
    {
        { Global.EnemyType.Slime,    1 },
        { Global.EnemyType.Wolf,     2 },
        { Global.EnemyType.Goblin,   3 },
        { Global.EnemyType.Dragon,   4 },
        { Global.EnemyType.Skeleton, 5 },
        { Global.EnemyType.Viking,   6 },
        { Global.EnemyType.Demon,    7 },
        { Global.EnemyType.Giant,    8 }
    };

    public static Dictionary<Global.EnemyType, List<float>> waveChances = new()
    {                                               // dummy   1     2     3     4     5
        { Global.EnemyType.Slime,    new List<float> { 0.0f, 0.7f, 0.1f, 0.0f,  0.0f, 0.0f } },
        { Global.EnemyType.Wolf,     new List<float> { 0.0f, 0.2f, 0.1f, 0.0f,  0.0f, 0.0f } },
        { Global.EnemyType.Goblin,   new List<float> { 0.0f, 0.1f, 0.6f, 0.1f,  0.0f, 0.0f } },
        { Global.EnemyType.Dragon,   new List<float> { 0.0f, 0.0f, 0.2f, 0.1f,  0.2f, 0.0f } },
        { Global.EnemyType.Skeleton, new List<float> { 0.0f, 0.0f, 0.0f, 0.4f,  0.4f, 0.2f } },
        { Global.EnemyType.Viking,   new List<float> { 0.0f, 0.0f, 0.0f, 0.4f, 0.25f, 0.3f } },
        { Global.EnemyType.Demon,    new List<float> { 0.0f, 0.0f, 0.0f, 0.0f, 0.13f, 0.3f } },
        { Global.EnemyType.Giant,    new List<float> { 0.0f, 0.0f, 0.0f, 0.0f, 0.02f, 0.2f } }
    };

                                               // dummy,  1,  2,  3,  4,  5
    public static List<int> baseTargetCosts = new() { 0, 10, 20, 30, 40, 50 };

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            Spawn(1);
        }
    }

    public void Spawn(int currentWave)
    {
        GetWaveNumberAndIteration(currentWave, out int waveNumber, out int iteration);
        List<EnemySpawnData> enemiesToBeSpawned = GenerateWave(waveNumber, iteration);

        StartCoroutine(SpawnWithDelay(enemiesToBeSpawned));
    }

    private void GetWaveNumberAndIteration(int currentWave, out int waveNumber, out int iteration)
    {
        iteration = currentWave / Global.MAX_WAVES + 1;
        waveNumber = currentWave % Global.MAX_WAVES;

        if (waveNumber == 0)
        {
            waveNumber = Global.MAX_WAVES;
            iteration--;
        }
    }

    private List<EnemySpawnData> GenerateWave(int waveNumber, int iteration)
    {
        List<EnemySpawnData> wave = new();

        int targetCost = baseTargetCosts[waveNumber] * iteration;
        int costSoFar = 0;

        while (costSoFar < targetCost)
        {
            EnemySpawnData enemyData = GetWeightedRandom(waveNumber);

            if (costSoFar + enemyData.GetCost() <= targetCost)
            {
                wave.Add(enemyData);
                costSoFar += enemyData.GetCost();
            }

            // can tolerate a small error. If the error is too high, get random enemy data again
            else if (5 * (targetCost - costSoFar) <= baseTargetCosts[waveNumber])
            {
                //print($"target: {targetCost} | current: {costSoFar} | baseTarget: {baseTargetCosts[waveNumber]}");
                break;
            }
        }

        return wave;
    }

    private IEnumerator SpawnWithDelay(List<EnemySpawnData> enemies)
    {
        foreach (EnemySpawnData enemyData in enemies)
        {
            GameObject enemyPrefab = Resources.Load<GameObject>
                ($"Prefabs/Enemies/{baseCosts[enemyData.type]}_{enemyData.type}/{enemyData.element}_{enemyData.type}");

            //print($"Prefabs/Enemies/{baseCosts[enemyData.type]}_{enemyData.type}/{enemyData.element}_{enemyData.type}");

            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            Enemy enemy = enemyInstance.GetComponent<Enemy>();

            enemy.waypoints = waypoints;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private EnemySpawnData GetWeightedRandom(int waveNumber)
    {
        // totalWeight should be 1 always, but just to be sure
        float totalWeight = 0f;

        foreach (var kvp in waveChances)
        {
            totalWeight += kvp.Value[waveNumber];
        }

        float randomValue = Random.Range(0f, totalWeight);

        foreach (var kvp in waveChances)
        {
            randomValue -= kvp.Value[waveNumber];

            if (randomValue <= 0f)
            {
                // 25% chance to be each element, including None
                Global.Element element = (Global.Element)Random.Range(0, 4);

                return new EnemySpawnData
                {
                    type = kvp.Key,
                    element = element
                };
            }
        }

        // Fallback (nu ar trebui sa ajunga aici)
        return new EnemySpawnData
        {
            type = Global.EnemyType.Slime,
            element = Global.Element.None
        };
    }
}