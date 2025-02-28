using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu2Story : MonoBehaviour
{
    // Method to change the scene to "Tutorial1"
    public void ChangeToTutorialScene()
    {
        SceneManager.LoadScene("Tutorial1");
    }
}
