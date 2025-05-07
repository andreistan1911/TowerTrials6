using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LossMenuHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI recordText;
    public Button backToMenuButton;
    public Button retryButton;

    private void Start()
    {
        retryButton.onClick.AddListener(OnRetry);
        backToMenuButton.onClick.AddListener(OnBackToMenu);

        int waveReached = PlayerPrefs.GetInt("LastWave", 0);
        int bestWave = PlayerPrefs.GetInt("BestWave", 0);

        SetupTexts(waveReached, bestWave);
    }

    public void SetupTexts(int waveReached, int bestWave)
    {
        titleText.text = "You lost!";
        waveText.text = "Wave reached: " + waveReached;

        if (waveReached > bestWave)
        {
            recordText.text = "New record!";
            PlayerPrefs.SetInt("BestWave", waveReached);
        }
        else
            recordText.text = "Your record: " + bestWave;
    }

    private void OnRetry()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("LostLevelName"));
    }

    private void OnBackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
