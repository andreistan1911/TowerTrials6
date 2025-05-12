using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManagerTutorial : AbstractWaveManager
{
    public List<GameObject> enemiesToSpawn;

    public override void Spawn()
    {
        WaveStateManager.Instance.OnWaveStarted();
        foreach (GameObject enemy in enemiesToSpawn)
            enemy.SetActive(true);

        StartCoroutine(WaitUntilNoEnemiesAndCallEnd());
    }

    public override List<EnemySpawnData> GetNextWaveEnemies()
    {
        return null; // should be useless
    }

    private IEnumerator WaitUntilNoEnemiesAndCallEnd()
    {
        while (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length > 0)
            yield return null;

        WaveStateManager.Instance.OnLastWaveEnded();
    }
}
