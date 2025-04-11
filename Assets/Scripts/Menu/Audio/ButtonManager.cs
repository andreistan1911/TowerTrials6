using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    void Start()
    {
        // Find all buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>();

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
