using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class ElementalOutline : MonoBehaviour
{
    private Image hpbarBackground;

    private static Color noneColor;
    private static Color fireColor;
    private static Color lightningColor;
    private static Color waterColor;

    private static bool colorsAreLoaded = false;

    private void Awake()
    {
        LoadColorsIfNeeded();
        hpbarBackground = transform.Find("Canvas").Find("HealthBar").Find("Background").GetComponent<Image>();
    }

    private void Start()
    {
        Assert.IsNotNull(hpbarBackground);
    }

    private static void LoadColorsIfNeeded()
    {
        if (colorsAreLoaded)
            return;

        noneColor      = Color.black;
        fireColor      = new(1.0f, 0.89f, 0.1568f);
        lightningColor = new(0.4157f, 0.0f, 0.4745f);
        waterColor     = new(0.0f, 0.6784f, 0.6353f);

        colorsAreLoaded = true;
    }

    public void SetHpBarColor(Global.Element element)
    {
        hpbarBackground.color = element switch
        {
            Global.Element.None => noneColor,
            Global.Element.Fire => fireColor,
            Global.Element.Lightning => lightningColor,
            Global.Element.Water => waterColor,
            _ => noneColor
        };
    }
}
