using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 20;

    private int health;
    private TextMeshProUGUI healthText;

    private void Start()
    {
        healthText = GameObject.Find("BaseHP Text").GetComponent<TextMeshProUGUI>();

        healthText.text = maxHealth.ToString();
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy == null)
            return;

        Destroy(other.gameObject);
        TakeDamage((int)enemy.type + 1);
    }

    private void OnCollisionEnter(Collision collision)
    {

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
        // TODO: IMPROVE THIS
        Time.timeScale = 0f; // opreste timpul
        Global.UnlockCursor();
        SceneManager.LoadScene("Main Menu");
    }
}
