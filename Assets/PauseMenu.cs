using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0.001f;
        Global.isPaused = true;
        Global.UnlockCursor();
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        Global.isPaused = false;
        Global.LockCursor();
    }
}
