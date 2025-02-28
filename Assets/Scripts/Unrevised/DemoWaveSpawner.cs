using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWaveSpawner : MonoBehaviour
{
    private List<Wave> waves = new();
    private float timer;

    private void Start()
    {
        timer = 0f;
        GetWaves();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        foreach (Wave wave in waves)
        {
            if (timer > wave.timer)
            {
                wave.gameObject.SetActive(true);
            }
        }
    }

    private void GetWaves()
    {
        foreach (Transform child in transform)
        {
            Wave wave = child.GetComponent<Wave>();

            if (wave != null)
                waves.Add(wave);
        }
    }
}
