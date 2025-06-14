using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveStateManager : MonoBehaviour
{
    public static WaveStateManager Instance { get; private set; }
    public static AbstractWaveManager waveManager;
    public Global.WinCode winCode;

    private static GameObject nextWaveImage;

    public enum WavePhase
    {
        BeforeWaveTime,
        DuringWaveTime,
        AfterLastWave
    }

    public WavePhase CurrentPhase { get; private set; } = WavePhase.BeforeWaveTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            waveManager = FindFirstObjectByType<AbstractWaveManager>();
            nextWaveImage = GameObject.Find("NextWave Image");
        }
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
    }

    public void OnAllEnemiesDefeated()
    {
        CurrentPhase = WavePhase.BeforeWaveTime;

        waveManager.SetANewWave();
        if (nextWaveImage != null)
            nextWaveImage.SetActive(true);
    }

    public void OnLastWaveEnded()
    {
        CurrentPhase = WavePhase.AfterLastWave;

        Global.HandleWin(winCode);
    }
}