using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManagerStory : AbstractWaveManger
{
    public Global.Level level;

    private static readonly Dictionary<Global.Level, Dictionary<int, List<EnemySpawnData>>> waves = new()
    {
        { Global.Level.Level_1, new Dictionary<int, List<EnemySpawnData>>
            {
                { 1, new List<EnemySpawnData>
                    {
                        new EnemySpawnData(Global.EnemyType.Slime, Global.Element.None),
                        new EnemySpawnData(Global.EnemyType.Goblin, Global.Element.None),
                        new EnemySpawnData(Global.EnemyType.Slime, Global.Element.None)
                    }
                },
                { 2, new List<EnemySpawnData>
                    {
                        new EnemySpawnData(Global.EnemyType.Wolf, Global.Element.None),
                        new EnemySpawnData(Global.EnemyType.Wolf, Global.Element.None)
                    }
                },
                { 3, new List<EnemySpawnData>
                    {
                        new EnemySpawnData(Global.EnemyType.Slime, Global.Element.Fire),
                        new EnemySpawnData(Global.EnemyType.Slime, Global.Element.Water)
                    }
                }
            }
        }
    };

    public override void Spawn(int currentWave)
    {
        Dictionary<int, List<EnemySpawnData>> levelWaves = waves[level];

        if (!levelWaves.ContainsKey(currentWave))
        {
            Debug.LogWarning("Wave not found!");
            return;
        }

        bool isLastWave = currentWave == levelWaves.Count;

        StartCoroutine(SpawnWave(levelWaves[currentWave], isLastWave));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Spawn(2);
        }
    }
}
