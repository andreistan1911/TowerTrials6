using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public float maxHealth;

    [HideInInspector]
    public float currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        GoldManager.GainGold((int)GetComponent<Enemy>().type + 1);
        Destroy(gameObject);
    }
}