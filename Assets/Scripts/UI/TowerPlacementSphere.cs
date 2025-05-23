using UnityEngine;

public class TowerPlacementSphere : MonoBehaviour
{
    [HideInInspector]
    public Tower tower;

    private GameObject crosshair;

    private void Start()
    {
        tower = null;
        crosshair = GameObject.Find("Crosshair");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TowerPurchaseMenu.Instance != null)
        {
            if (crosshair != null)
                crosshair.SetActive(false);
            TowerPurchaseMenu.Instance.gameObject.SetActive(true);
            TowerPurchaseMenu.Instance.SetPositionToBuild(transform.parent.position);
            TowerPurchaseMenu.Instance.purchaseSphere = this;
        }

        // Show and unlock the cursor
        Global.UnlockCursor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (TowerPurchaseMenu.Instance != null)
        {
            if (crosshair != null)
                crosshair.SetActive(true);
            TowerPurchaseMenu.Instance.ResetSelections();
            TowerPurchaseMenu.Instance.gameObject.SetActive(false);
        }

        // Hide and lock the cursor
        Global.LockCursor();
    }
}
