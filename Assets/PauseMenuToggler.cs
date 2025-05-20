using UnityEngine;

public class PauseMenuToggler : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject crosshair;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = pauseMenu.activeSelf;

            pauseMenu.SetActive(!isActive);
            crosshair.SetActive(isActive);
        }
    }
}
