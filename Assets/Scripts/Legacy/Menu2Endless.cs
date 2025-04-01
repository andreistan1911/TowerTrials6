using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu2Endless : MonoBehaviour
{
        // Method to change the scene to "Tutorial1"
    public void ChangeToEndless()
    {
            SceneManager.LoadScene("Endless");
    }

}
