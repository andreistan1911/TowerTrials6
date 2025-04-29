using System.Collections;
using UnityEngine;

public class HitscanWeapon : MonoBehaviour
{
    [Header("Hitscan Settings")]
    public float range;
    public float damage;

    public LayerMask layersToIgnore;

    private LineRenderer beam;

    private void Awake()
    {
        beam = GetComponent<LineRenderer>();
    }

    public void Fire()
    {
        print(transform.position);

        beam.SetPosition(0, transform.position);
        beam.SetPosition(1, transform.position);

        Vector3 shootDirection = transform.forward;

        if (Physics.Raycast(transform.position, shootDirection, out RaycastHit hit, range, ~layersToIgnore))
        {
            Debug.Log("Lovit: " + hit.collider.name);

            // Damage
            Health enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Visual effect
            if (beam != null)
            {
                ShowLightning(hit.point);
            }
        }
        else
        {
            if (beam != null)
            {
                ShowLightning(transform.position + shootDirection * range);
            }
        }
    }

    private void ShowLightning(Vector3 targetPoint)
    {
        beam.SetPosition(1, transform.position);
        beam.SetPosition(1, targetPoint);
    }
}
