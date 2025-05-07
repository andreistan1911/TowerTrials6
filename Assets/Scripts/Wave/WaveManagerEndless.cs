using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerEndless : AbstractWaveManager
{
    private static readonly Dictionary<Global.EnemyType, List<float>> waveChances = new()
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
    private static readonly List<int> baseTargetCosts = new() { 0, 10, 20, 30, 40, 50 };

    public override void Spawn()
    {
        StartCoroutine(SpawnWave(enemiesToBeSpawned));
    }

    public override List<EnemySpawnData> GetNextWaveEnemies()
    {
        GetWaveNumberAndIteration(currentWave, out int waveNumber, out int iteration);

        if (waveNumber <= 5)
            return GenerateWave(waveNumber, iteration);

        else if (waveNumber == 6)
            return new List<EnemySpawnData>
                    {
                        new EnemySpawnData(Global.EnemyType.DragonMama, Global.Element.None),
                    };

        else //if (waveNumber == 7)
            return new List<EnemySpawnData>
                    {
                        new EnemySpawnData(Global.EnemyType.Wizard, Global.Element.None),
                    };
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

                return new EnemySpawnData(kvp.Key, element);
            }
        }

        // Fallback (nu ar trebui sa ajunga aici)
        return new EnemySpawnData(Global.EnemyType.Slime, Global.Element.None);
    }
}