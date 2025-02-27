using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (player != null)
        {
            // Billboard should always be 'looking' towards player
            transform.LookAt(transform.position + player.forward);
        }
    }
}
