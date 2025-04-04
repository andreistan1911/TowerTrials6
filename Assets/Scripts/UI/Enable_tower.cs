using UnityEngine;

public class BuildTowerButton : MonoBehaviour
{
    [SerializeField] private GameObject towerToEnable;
    [SerializeField] private GameObject zoneToDisable;
    [SerializeField] private GameObject buttonToDisable;

    public void OnBuildTowerClicked()
    {
        if (towerToEnable != null)
            towerToEnable.SetActive(true);

        if (zoneToDisable != null)
            zoneToDisable.SetActive(false);

        if (buttonToDisable != null)
            buttonToDisable.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
