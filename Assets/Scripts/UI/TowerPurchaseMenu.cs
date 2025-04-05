using UnityEngine;
using UnityEngine.UI;

public class TowerPurchaseMenu : MonoBehaviour
{
    public Button fireButton, lightningButton, waterButton;
    public Button aoeButton, bombButton, singleButton, continuousButton;
    public Button confirmButton;
    public GameObject ShadowingPanel;

    public Image dmgBar, rangeBar, atkSpeedBar;
    public Tower towerToBuild;
    public GameObject purchaseZone;

    private string selectedElement = null;
    private string selectedType = null;

    private void Start()
    {
        // Initializare
        ResetSelections();
        confirmButton.onClick.AddListener(OnBuildTowerClicked);

        fireButton.onClick.AddListener(() => SelectElement("Fire"));
        lightningButton.onClick.AddListener(() => SelectElement("Lightning"));
        waterButton.onClick.AddListener(() => SelectElement("Water"));

        aoeButton.onClick.AddListener(() => SelectType("Aoe"));
        bombButton.onClick.AddListener(() => SelectType("Bomb"));
        singleButton.onClick.AddListener(() => SelectType("Single"));
        continuousButton.onClick.AddListener(() => SelectType("Continuous"));
    }

    private void SelectElement(string element)
    {
        selectedElement = element;
        UpdateStatsDisplay();
        UpdateConfirmButtonState();
    }

    private void SelectType(string type)
    {
        selectedType = type;
        UpdateStatsDisplay();
        UpdateConfirmButtonState();
    }

    private void UpdateStatsDisplay()
    {
        if (selectedElement == null || selectedType == null)
        {
            SetBarFill(dmgBar, 0);
            SetBarFill(rangeBar, 0);
            SetBarFill(atkSpeedBar, 0);
            return;
        }

        float dmg, range, atkSpeed;

        dmg = 0.8f; //TODO: SHOULD HAVE ACTUAL DB VALUES
        range = 0.3f; //TODO: SHOULD HAVE ACTUAL DB VALUES
        atkSpeed = 0.6f;//5 / towerToBuild.attackRate; // TODO: sau cat o fi maxim de atk speed

        SetBarFill(dmgBar, dmg);
        SetBarFill(rangeBar, range);
        SetBarFill(atkSpeedBar, atkSpeed);
    }

    private void SetBarFill(Image bar, float value)
    {
        bar.fillAmount = Mathf.Clamp01(value);
    }

    private void UpdateConfirmButtonState()
    {
        bool interactable = selectedElement != null && selectedType != null;

        print(interactable);
        ShadowingPanel.SetActive(!interactable);
    }


    public void OnBuildTowerClicked()
    {
        if (towerToBuild != null)
            towerToBuild.gameObject.SetActive(true);

        if (purchaseZone != null)
            purchaseZone.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResetSelections();

        gameObject.SetActive(false);
    }

    private void ResetSelections()
    {
        selectedElement = null;
        selectedType = null;
        UpdateStatsDisplay();
        UpdateConfirmButtonState();
    }
}
