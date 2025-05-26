using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerPurchaseMenu : MonoBehaviour
{
    public Button elementlessButton, fireButton, lightningButton, waterButton;
    public Button aoeButton, bombButton, singleButton, laserButton;
    public Button confirmButton;

    public Image dmgBar, rangeBar, atkSpeedBar;
    public TextMeshProUGUI costText;
    public Image selectedElementImage;
    public TextMeshProUGUI selectedTypeText;

    [HideInInspector]
    public TowerPlacementSphere purchaseSphere;

    [HideInInspector]
    public Global.Element selectedElement = Global.Element.None;
    private Global.TowerType selectedType = Global.TowerType.Single;

    private Vector3 positionToBuild;

    private GameObject crosshair;

    [HideInInspector]
    public bool AmIinTutorial = false;

    public static TowerPurchaseMenu Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
    }

    private void Start()
    {
        if (name == "Tutorial Menu")
            AmIinTutorial = true;

        SetBarFill(dmgBar, 0);
        SetBarFill(rangeBar, 0);
        SetBarFill(atkSpeedBar, 0);
        ResetSelections();
        confirmButton.onClick.AddListener(BuildTower);

        elementlessButton.onClick.AddListener(() => SelectElement(Global.Element.None));
        fireButton.onClick.AddListener(() => SelectElement(Global.Element.Fire));
        lightningButton.onClick.AddListener(() => SelectElement(Global.Element.Lightning));
        waterButton.onClick.AddListener(() => SelectElement(Global.Element.Water));

        aoeButton.onClick.AddListener(() => SelectType(Global.TowerType.Aoe));
        bombButton.onClick.AddListener(() => SelectType(Global.TowerType.Bomb));
        singleButton.onClick.AddListener(() => SelectType(Global.TowerType.Single));
        laserButton.onClick.AddListener(() => SelectType(Global.TowerType.Laser));

        crosshair = GameObject.Find("Crosshair");

        gameObject.SetActive(false);
    }

    private void SelectElement(Global.Element element)
    {
        selectedElement = element;
        UpdateStatsDisplay();
        UpdateSelectedElement();
    }

    private void SelectType(Global.TowerType type)
    {
        selectedType = type;
        UpdateStatsDisplay();
        UpdateSelectedType();
    }

    private void UpdateStatsDisplay()
    {
        float dmg, range, atkSpeed;

        dmg = Global.towerValues[selectedElement][selectedType].damage / Global.maxTowerDamage;
        range = Global.towerValues[selectedElement][selectedType].range / Global.maxTowerRange;
        atkSpeed = 30f / (Global.maxTowerAtkSpd * Global.towerValues[selectedElement][selectedType].attackRate);

        costText.text = $"Cost: {Global.towerValues[selectedElement][selectedType].cost}";
        SetBarFill(dmgBar, dmg);
        SetBarFill(rangeBar, range);
        SetBarFill(atkSpeedBar, atkSpeed);
    }

    private void SetBarFill(Image bar, float value)
    {
        bar.fillAmount = Mathf.Clamp01(value);
    }

    private void UpdateSelectedElement()
    {
        if (AmIinTutorial)
            selectedElement = Global.Element.Fire;

        switch (selectedElement)
        {
            case Global.Element.None:
                selectedElementImage.sprite = elementlessButton.GetComponent<Image>().sprite;
                selectedElementImage.color = elementlessButton.GetComponent<Image>().color;
                break;
            case Global.Element.Fire:
                selectedElementImage.sprite = fireButton.GetComponent<Image>().sprite;
                selectedElementImage.color = fireButton.GetComponent<Image>().color;
                break;
            case Global.Element.Lightning:
                selectedElementImage.sprite = lightningButton.GetComponent<Image>().sprite;
                selectedElementImage.color = lightningButton.GetComponent<Image>().color;
                break;
            case Global.Element.Water:
                selectedElementImage.sprite = waterButton.GetComponent<Image>().sprite;
                selectedElementImage.color = waterButton.GetComponent<Image>().color;
                break;

            default:
                selectedElementImage.sprite = elementlessButton.GetComponent<Image>().sprite;
                selectedElementImage.color = elementlessButton.GetComponent<Image>().color;
                break;
        }
    }

    private void UpdateSelectedType()
    {
        if (AmIinTutorial)
            selectedType = Global.TowerType.Single;

        selectedTypeText.text = selectedType switch
        {
            Global.TowerType.Aoe => "Aoe",
            Global.TowerType.Bomb => "Bomb",
            Global.TowerType.Single => "Single target",
            Global.TowerType.Laser => "Laser",
            _ => "Single target"
        };
    }

    public void BuildTower()
    {
        if (!GoldManager.SpendGold(Global.towerValues[selectedElement][selectedType].cost))
            return;

        // Return default cursor proprieties
        Global.LockCursor();

        // Instantiate tower
        string towerType = selectedType.ToString();
        string towerElement = selectedElement.ToString();

        GameObject towerPrefab = Resources.Load<GameObject>("Prefabs/Towers/Tower" + towerType + towerElement);
        GameObject towerInstance = Instantiate(towerPrefab, positionToBuild, Quaternion.identity);
        Tower tower = towerInstance.GetComponentInChildren<Tower>();

        if (purchaseSphere != null)
        {
            if (purchaseSphere.tower != null)
            {
                GoldManager.GainGold((int)(0.3f * Global.towerValues[purchaseSphere.tower.element][purchaseSphere.tower.GetTowerTypeFromName()].cost));
                Destroy(purchaseSphere.tower.gameObject.transform.parent.gameObject);
            }

            purchaseSphere.tower = tower;
        }

        // Reset element & type and disable build menu
        ResetSelections();
        if (crosshair != null)
            crosshair.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ResetSelections()
    {
        selectedElement = Global.Element.None;
        selectedType = Global.TowerType.Single;
        costText.text = "Cost:";
        UpdateStatsDisplay();
        UpdateSelectedElement();
        UpdateSelectedType();
    }

    public void SetPositionToBuild(Vector3 position)
    {
        positionToBuild = position;
    }
}
