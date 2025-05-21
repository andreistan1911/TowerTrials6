using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static TextMeshProUGUI goldText;

    private void Start()
    {
        goldText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private static int GetGold()
    {
        return int.Parse(goldText.text);
    }

    private static void SetGold(int gold)
    {
        goldText.text = gold.ToString();
    }

    public static void GainGold(int gold)
    {
        if (goldText == null)
            return;

        SetGold(GetGold() + gold);
    }

    public static bool SpendGold(int cost)
    {
        int gold = GetGold();

        if (gold - cost < 0)
            return false;

        SetGold(gold - cost);
        return true;
    }
}
