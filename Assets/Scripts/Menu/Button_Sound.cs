using UnityEngine;
using UnityEngine.UI;  // For accessing UI components

public class ButtonSound : MonoBehaviour
{
    public Button myButton;  
    public AudioSource audioSource;  
    public AudioClip clickSound; 

    void Start()
    {
        
        myButton.onClick.AddListener(PlaySound);
    }

    
    void PlaySound()
    {
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound); 
        }
    }
}
