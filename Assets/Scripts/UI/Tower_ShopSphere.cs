using UnityEngine;

public class ZoneActivator : MonoBehaviour
{
    [SerializeField] private GameObject buttonToToggle;

    private void OnTriggerEnter(Collider other)
    {
            if (buttonToToggle != null)
                buttonToToggle.SetActive(true);

            // Show and unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }

    private void OnTriggerExit(Collider other)
    {
            if (buttonToToggle != null)
                buttonToToggle.SetActive(false);

            // Hide and lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    }
}
