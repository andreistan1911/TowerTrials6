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
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, Global.Element.Lightning);
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

    /*private void ShowLightning(Vector3 targetPoint)
    {
        beam.SetPosition(1, transform.position);
        beam.SetPosition(1, targetPoint);
    }*/

    private void ShowLightning(Vector3 targetPoint)
    {
        Vector3 startPoint = transform.position;
        Vector3 direction = (targetPoint - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, targetPoint);
        float segmentLength = 0.2f;
        int segments = Mathf.Max(4, Mathf.RoundToInt(distance / segmentLength));
        float maxOffset = 0.1f;

        beam.positionCount = segments + 1;

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up);
        if (perpendicular == Vector3.zero)
            perpendicular = Vector3.Cross(direction, Vector3.right);

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            Vector3 point = Vector3.Lerp(startPoint, targetPoint, t);

            // Doar punctele intermediare au zigzag
            if (i != 0 && i != segments)
            {
                float falloff = Mathf.Sin(t * Mathf.PI);
                float offset = Random.Range(-maxOffset, maxOffset) * falloff;

                Vector3 offsetDir = Quaternion.AngleAxis(Random.Range(-45f, 45f), direction) * perpendicular;
                point += offsetDir * offset;
            }

            beam.SetPosition(i, point);
        }
    }
}
