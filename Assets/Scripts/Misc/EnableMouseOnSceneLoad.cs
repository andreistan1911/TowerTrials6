using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableMouseOnSceneLoad : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to scene load events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled or destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnableMouse();
    }

    private void Start()
    {
        // Ensure mouse is enabled at the start too (for the initial scene)
        EnableMouse();
    }

    private void EnableMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
