using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private Transform player; // Reference to the player's camera
    private GameObject healthBar;    // Reference to the health bar GameObject

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        healthBar = transform.Find("Canvas/HealthBar").gameObject;
    }

    private void Update()
    {
        Vector3 directionToEnemy = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, directionToEnemy);

        if (angle <  Global.viewAngle)
            healthBar.SetActive(true);
        else
            healthBar.SetActive(false);
    }
}
