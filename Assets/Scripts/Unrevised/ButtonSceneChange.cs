using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName = "Menu";

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

}
