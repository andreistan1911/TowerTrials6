using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManagerStory : AbstractWaveManager
{
    public Global.Level level;

    private static readonly Dictionary<Global.Level, Dictionary<int, List<EnemySpawnData>>> waves = new()
    {
        { Global.Level.Tutorial, new Dictionary<int, List<EnemySpawnData>>
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
                },
                { 4, new List<EnemySpawnData>
                    {
                        new EnemySpawnData(Global.EnemyType.Goblin, Global.Element.None),
                        new EnemySpawnData(Global.EnemyType.Goblin, Global.Element.Lightning),
                        new EnemySpawnData(Global.EnemyType.Goblin, Global.Element.Lightning),
                    }
                },
                { 5, new List<EnemySpawnData>
                    {
                        new EnemySpawnData(Global.EnemyType.Wolf, Global.Element.Lightning),
                        new EnemySpawnData(Global.EnemyType.Slime, Global.Element.Water),
                        new EnemySpawnData(Global.EnemyType.Slime, Global.Element.Fire),
                        new EnemySpawnData(Global.EnemyType.Slime, Global.Element.Lightning),
                    }
                }
            }
        }
    };

    public override void Spawn()
    {
        Dictionary<int, List<EnemySpawnData>> levelWaves = waves[level];
        bool isLastWave = currentWave == levelWaves.Count;

        StartCoroutine(SpawnWave(levelWaves[currentWave], isLastWave));
    }

    public override List<EnemySpawnData> GetNextWaveEnemies()
    {
        return waves[level][currentWave];
    }
}
