using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform rectTransform;
    private Health health;
    private Image fillImage;
    private float maxWidth;

    private void Start()
    {
        rectTransform = transform.Find("Fill").GetComponent<RectTransform>();
        maxWidth = rectTransform.rect.width;

        fillImage = transform.Find("Fill").GetComponent<Image>();
        health = transform.parent.parent.GetComponent<Health>();

        if (fillImage == null)
            Debug.LogError("No fill image!");

        if (health == null)
            Debug.LogError("No health script!");
    }

    private void Update()
    {
        UpdateHealthBar(health.currentHealth);
    }

    private void UpdateHealthBar(float currentHealth)
    {
        float healthPercentage = currentHealth / health.maxHealth;
        rectTransform.sizeDelta = new Vector2(maxWidth * healthPercentage, rectTransform.sizeDelta.y);
    }
}
