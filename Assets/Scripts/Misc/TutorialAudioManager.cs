using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialAudioManager : MonoBehaviour
{
    private static TutorialAudioManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist this object across scene loads

        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Endless" || scene.name == "Main Menu")
        {
            Destroy(gameObject); // Stop the tutorial audio permanently
        }
    }
}
