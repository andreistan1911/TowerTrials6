using UnityEngine;

public class EndlessButtonAvailability : MonoBehaviour
{
    [SerializeField]
    private GameObject endlessText;

    [SerializeField]
    private GameObject lockImage;

    private void Start()
    {
        Global.UnlockCursor(); // should have been on any object on scene
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("EndlessUnlocked") == 1)
        {
            endlessText.SetActive(true);
            lockImage.SetActive(false);
        }
        else
        {
            endlessText.SetActive(false);
            lockImage.SetActive(true);
        }
    }
}
