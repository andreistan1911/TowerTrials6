using UnityEngine;

public class TowerPlacementSphere : MonoBehaviour
{
    public Tower tower; //TODO: SHOULD REPLACE WITH INSTANCIATING PREFABS INSTEAD

    private TowerPurchaseMenu menu;

    private void Awake()
    {
        menu = FindFirstObjectByType<TowerPurchaseMenu>();
        menu.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (menu != null)
        {
            menu.gameObject.SetActive(true);
            menu.towerToBuild = tower;
            menu.purchaseZone = gameObject;
        }

        // Show and unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (menu != null)
            menu.gameObject.SetActive(false);

        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
