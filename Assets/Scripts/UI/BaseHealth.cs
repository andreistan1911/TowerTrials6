using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 20;

    private int health;
    private TextMeshProUGUI healthText;
    private AbstractWaveManager waveManager;

    private void Start()
    {
        healthText = GameObject.Find("BaseHP Text").GetComponent<TextMeshProUGUI>();
        waveManager = FindFirstObjectByType<AbstractWaveManager>();

        healthText.text = maxHealth.ToString();
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy == null)
            return;

        Destroy(other.gameObject);
        TakeDamage(GetDamageByType(enemy.type));
    }

    private int GetDamageByType(Global.EnemyType type)
    {
        return type switch
        {
            Global.EnemyType.Slime => 1,
            Global.EnemyType.Wolf => 2,
            Global.EnemyType.Goblin => 2,
            Global.EnemyType.Dragon => 2,
            Global.EnemyType.Skeleton => 3,
            Global.EnemyType.Viking => 4,
            Global.EnemyType.Demon => 5,
            Global.EnemyType.Giant => 5,
            Global.EnemyType.DragonMama => 10,
            Global.EnemyType.Wizard => 100,
            _ => 1,
        };
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = health.ToString();

        if (health <= 0)
        {
            DoLoseLogic();
        }
    }

    private void DoLoseLogic()
    {
        Global.UnlockCursor();

        PlayerPrefs.SetInt("LastWave", waveManager.currentWave);
        PlayerPrefs.SetString("LostLevelName", SceneManager.GetActiveScene().name);

        SceneManager.LoadScene("Loss Endless");
    }
}
