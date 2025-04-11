using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    private void Start()
    {
        // Find all buttons in the scene
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() =>
            {
                if (AudioManager.instance != null)
                    AudioManager.instance.PlayClickSound();
            });
        }
    }
}
