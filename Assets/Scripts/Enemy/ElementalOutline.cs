using UnityEngine;
using UnityEngine.Assertions;

public class ElementalOutline : MonoBehaviour
{
    public MeshRenderer objectToOutline;

    private static Material noneOutlineMat;
    private static Material fireOutlineMat;
    private static Material lightningOutlineMat;
    private static Material waterOutlineMat;

    private static bool materialsLoaded = false;

    private void Awake()
    {
        LoadMaterialsIfNeeded();
    }

    private void Start()
    {
        Assert.IsNotNull(objectToOutline);
    }

    private static void LoadMaterialsIfNeeded()
    {
        if (materialsLoaded)
            return;

        noneOutlineMat      = Resources.Load<Material>("Materials/NoneOutlineMat");
        fireOutlineMat      = Resources.Load<Material>("Materials/FireOutlineMat");
        lightningOutlineMat = Resources.Load<Material>("Materials/LightningOutlineMat");
        waterOutlineMat     = Resources.Load<Material>("Materials/WaterOutlineMat");

        materialsLoaded = true;
    }

    public void SetOutlineElement(Global.Element element)
    {
        Material[] materials = objectToOutline.materials;

        if (materials.Length < 2)
        {
            Debug.LogError($"Object {gameObject.name} does not have enough materials to set an outline");
            return;
        }

        Material outlineMat = noneOutlineMat;

        switch (element)
        {
            case Global.Element.Fire:
                outlineMat = fireOutlineMat;
                break;
            case Global.Element.Lightning:
                outlineMat = lightningOutlineMat;
                break;
            case Global.Element.Water:
                outlineMat = waterOutlineMat;
                break;
            case Global.Element.None:
                outlineMat = noneOutlineMat;
                break;
            default:
                Debug.LogError("Undefined element!");
                break;
        }

        materials[1] = outlineMat;
        objectToOutline.materials = materials;
    }
}
