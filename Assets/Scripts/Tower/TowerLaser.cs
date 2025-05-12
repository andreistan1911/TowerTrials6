using UnityEngine;

public class TowerLaser : Tower
{
    private Transform eye;
    private LineRenderer beam;
    private float lastFire;

    private Vector3 defaultBeamPosition;

    private new void Start()
    {
        base.Start();

        lastFire = 0f;
        defaultBeamPosition = transform.position + new Vector3(0.01f, 4.4f, 0f);
        eye = transform.Find("Eye").transform;
        beam = GetComponent<LineRenderer>();

        beam.SetPosition(0, defaultBeamPosition);
        beam.SetPosition(1, defaultBeamPosition);
    }

    private void Update()
    {
        if (!IsEnemyInRange())
            beam.SetPosition(1, defaultBeamPosition);
    }

    private void Fire(GameObject target)
    {
        eye.LookAt(target.transform.position);
        beam.SetPosition(1, target.transform.position);
        // line renderer logic
    }

    override public void Fire(Enemy enemy)
    {
        if (Time.time - lastFire >= attackRate)
        {
            Fire(enemy.transform.Find("ShootRoot").gameObject);
            enemy.TakeDamage(damage, element);
            lastFire = Time.time;
        }
    }
}
