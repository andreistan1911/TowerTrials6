using UnityEngine;

public class WaveStateManager : MonoBehaviour
{
    public static WaveStateManager Instance { get; private set; }

    public enum WavePhase
    {
        BeforeWaveTime,
        DuringWaveTime,
        AfterLastWave
    }

    public WavePhase CurrentPhase { get; private set; } = WavePhase.BeforeWaveTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        int enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

        if (CurrentPhase == WavePhase.DuringWaveTime && enemyCount == 0)
        {
            OnAllEnemiesDefeated();
        }
    }

    public void OnWaveStarted()
    {
        CurrentPhase = WavePhase.DuringWaveTime;
        Debug.Log("Wave started");
    }

    public void OnAllEnemiesDefeated()
    {
        CurrentPhase = WavePhase.BeforeWaveTime;
        Debug.Log("All enemies defeated");
    }

    public void OnLastWaveEnded()
    {
        CurrentPhase = WavePhase.AfterLastWave;
        Debug.Log("Last wave ended");
    }
}