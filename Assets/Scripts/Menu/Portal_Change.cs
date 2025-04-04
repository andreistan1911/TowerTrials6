using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_Change : MonoBehaviour
{
    [SerializeField] private string sceneName = "Menu";

    private void OnTriggerEnter(Collider other)
    {

        SceneManager.LoadScene(sceneName);
    }
}
