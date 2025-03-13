using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform rectTransform;
    private Transform player;
    private Transform myParent;
    private Canvas canvas;
    private Health health;
    private Image fillImage;
    private float maxWidth;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        rectTransform = transform.Find("Fill").GetComponent<RectTransform>();
        maxWidth = rectTransform.rect.width;
        fillImage = transform.Find("Fill").GetComponent<Image>();
        canvas = transform.parent.GetComponent<Canvas>();
        myParent = transform.parent.parent;
        health = myParent.GetComponent<Health>();

        if (fillImage == null)
            Debug.LogError("No fill image!");

        if (health == null)
            Debug.LogError("No health script!");
    }

    private void Update()
    {
        UpdateHealthBar(health.currentHealth);
        UpdateVisibility();
    }

    private void UpdateHealthBar(float currentHealth)
    {
        float healthPercentage = currentHealth / health.maxHealth;
        rectTransform.sizeDelta = new Vector2(maxWidth * healthPercentage, rectTransform.sizeDelta.y);
    }

    private void UpdateVisibility()
    {
        Vector3 directionToEnemy = (myParent.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToEnemy);

        if (angle < Global.VIEW_ANGLE)
            canvas.enabled = true;
        else
            canvas.enabled = false;
    }
}
