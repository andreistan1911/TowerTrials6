using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName = "Menu";

    public void ChangeScene()
    {
        print(sceneName);
        SceneManager.LoadScene(sceneName);
    }

}
