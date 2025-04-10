using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerPurchaseMenu : MonoBehaviour
{
    public Button elementlessButton, fireButton, lightningButton, waterButton;
    public Button aoeButton, bombButton, singleButton, continuousButton;
    public Button confirmButton;

    public Image dmgBar, rangeBar, atkSpeedBar;
    public Image selectedElementImage;
    public TextMeshProUGUI selectedTypeText;

    [HideInInspector]
    public TowerPlacementSphere purchaseSphere;

    private const int NONE = 0;
    private const int FIRE = 1;
    private const int LIGHTNING = 2;
    private const int WATER = 3;

    private const int AOE = 4;
    private const int BOMB = 5;
    private const int SINGLE = 6;
    private const int CONTINOUS = 7;

    private int selectedElement = NONE;
    private int selectedType = SINGLE;

    private Vector3 positionToBuild;

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
        SetBarFill(dmgBar, 0);
        SetBarFill(rangeBar, 0);
        SetBarFill(atkSpeedBar, 0);

        ResetSelections();
        confirmButton.onClick.AddListener(BuildTower);

        elementlessButton.onClick.AddListener(() => SelectElement(NONE));
        fireButton.onClick.AddListener(() => SelectElement(FIRE));
        lightningButton.onClick.AddListener(() => SelectElement(LIGHTNING));
        waterButton.onClick.AddListener(() => SelectElement(WATER));

        aoeButton.onClick.AddListener(() => SelectType(AOE));
        bombButton.onClick.AddListener(() => SelectType(BOMB));
        singleButton.onClick.AddListener(() => SelectType(SINGLE));
        continuousButton.onClick.AddListener(() => SelectType(CONTINOUS));

        gameObject.SetActive(false);
    }

    private void SelectElement(int element)
    {
        selectedElement = element;
        UpdateStatsDisplay();
        UpdateSelectedElement();
    }

    private void SelectType(int type)
    {
        selectedType = type;
        UpdateStatsDisplay();
        UpdateSelectedType();
    }

    private void UpdateStatsDisplay()
    {
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

    private void UpdateSelectedElement()
    {
        switch (selectedElement)
        {
            case NONE:
                selectedElementImage.sprite = elementlessButton.GetComponent<Image>().sprite;
                selectedElementImage.color = elementlessButton.GetComponent<Image>().color;
                break;
            case FIRE:
                selectedElementImage.sprite = fireButton.GetComponent<Image>().sprite;
                selectedElementImage.color = fireButton.GetComponent<Image>().color;
                break;
            case LIGHTNING:
                selectedElementImage.sprite = lightningButton.GetComponent<Image>().sprite;
                selectedElementImage.color = lightningButton.GetComponent<Image>().color;
                break;
            case WATER:
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
        switch (selectedType)
        {
            case AOE:
                selectedTypeText.text = "Aoe";
                break;
            case BOMB:
                selectedTypeText.text = "Bomb";
                break;
            case SINGLE:
                selectedTypeText.text = "Single target";
                break;
            case CONTINOUS:
                selectedTypeText.text = "Laser";
                break;

            default:
                selectedTypeText.text = "Single target";
                break;
        }
    }

    public void BuildTower()
    {
        // Return default cursor proprieties
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Instantiate tower
        string towerType = GetStringType();
        string towerElement = GetStringElement();

        GameObject towerPrefab = Resources.Load<GameObject>("Prefabs/Towers/Tower" + towerType + towerElement);
        GameObject towerInstance = Instantiate(towerPrefab, positionToBuild, Quaternion.identity);
        Tower tower = towerInstance.GetComponentInChildren<Tower>();

        if (purchaseSphere != null)
        {
            if (purchaseSphere.tower != null)
            {
                Destroy(purchaseSphere.tower.gameObject.transform.parent.gameObject);
            }

            purchaseSphere.tower = tower;
        }

        // Reset element & type and disable build menu
        ResetSelections();
        gameObject.SetActive(false);
    }

    public void ResetSelections()
    {
        selectedElement = NONE;
        selectedType = SINGLE;
        UpdateStatsDisplay();
        UpdateSelectedElement();
        UpdateSelectedType();
    }

    private string GetStringType()
    {
        switch (selectedType)
        {
            case AOE:
                return "Aoe";
            case BOMB:
                return "Bomb";
            case SINGLE:
                return "Single";
            case CONTINOUS:
                return "Laser";

            default:
                Debug.LogError("Error building tower - string type");
                return "";
        }
    }

    private string GetStringElement()
    {
        switch (selectedElement)
        {
            case NONE:
                return "None";
            case FIRE:
                return "Fire";
            case LIGHTNING:
                return "Lightning";
            case WATER:
                return "Water";

            default:
                Debug.LogError("Error building tower - string element");
                return "";
        }
    }

    public void SetPositionToBuild(Vector3 position)
    {
        positionToBuild = position;
    }
}
