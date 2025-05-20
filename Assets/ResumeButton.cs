using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public void Resume()
    {
        GameObject.Find("PauseMenu").SetActive(false);
    }
}
