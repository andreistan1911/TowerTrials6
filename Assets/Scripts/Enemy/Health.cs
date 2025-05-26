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
        GoldManager.GainGold(GetGoldByType(GetComponent<Enemy>().type));
        Destroy(gameObject);
    }

    private int GetGoldByType(Global.EnemyType type)
    {
        return type switch
        {
            Global.EnemyType.Slime => 1,
            Global.EnemyType.Wolf => 2,
            Global.EnemyType.Goblin => 2,
            Global.EnemyType.Dragon => 3,
            Global.EnemyType.Skeleton => 4,
            Global.EnemyType.Viking => 4,
            Global.EnemyType.Demon => 5,
            Global.EnemyType.Giant => 5,
            Global.EnemyType.DragonMama => 20,
            Global.EnemyType.Wizard => 30,
            _ => 1,
        };
    }
}