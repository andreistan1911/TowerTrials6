using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    private float height;

    private float deltaHeightUp, heightInitial;
    private Vector3 velocity;
    private float velocityYInitial, velocityYInitialSquared;

    private int ascending = 1;
    private bool reachedTarget = false;
    private MeshRenderer meshRenderer;

    private ParticleSystem VFX;
    private float timeElapsedSinceExplosion = 0f;
    private float VFXtime;

    private Transform explosionTransform;
    private Explosion explosion;

    public float Height
    {
        set { height = value; }
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        VFX = GetComponentInChildren<ParticleSystem>();
        explosionTransform = VFX.transform.Find("Hitbox");
        explosion = explosionTransform.gameObject.GetComponent<Explosion>();

        VFX.Stop();
        VFXtime = VFX.main.duration + VFX.main.startLifetime.constant;

        explosion.Damage = damage;
        explosion.Element = element;

        PreCompute();
    }

    /// <summary>
    /// Computes velocities before launching the bomb. Details in comments.
    /// </summary>
    public void PreCompute()
    {
        /*
         *  In Y axis:
         *  Ec = m * v_i^2 / 2
         *  Ep + Ec = ct
         *  
         *  mgh + mv^2 / 2 = mv_i^2 / 2, where v is Y velocity at any time, h is the height that object should go above the starting height
         *  2gh + v^2 = v_i^2
         *  v_i^2 - v^2 = 2gh
         *  v = sqrt(v_i^2 - 2gh)
         *  
         *  v = 0 => v_i = sqrt(2gh)    -> found initial Y VELOCITY, given the height i throw the object
         *  --------------------------------------------------------------------------------------------
         *  t_up = v_i / g              -> found TIME_ASCEND = time for max height, which is also equal to the time required to fall to initial height
         *  ------------------------------------------------------------------------------------------------------------------------------------------
         *  
         *  m * v_f^2 / 2 = m * g * h_i + m * v_i^2 / 2, where v_f is final y velocity, h_i is initial height
         *  v_f^2 = 2 * g * h_i + v_i^2
         *  
         *  v_f = sqrt(2 * g * h_i + v_i^2)
         *  
         *  But v_f = v_i + g * t_down
         *  
         *  So  
         *  t_down = (sqrt(2 * g * h_i + v_i^2) - v_i) / g  -> found TIME_DESCEND = time to descend to final destination, after falling back to initial height
         *  And
         *  t_total = 2 * t_up + t_down                     -> found TOTAL TIME of object's balistic movement
         *  
         *  In XOZ plane:
         *  vx = delta_x / t_total
         *  vz = delta_z / t_total
         * */


        float _deltaHeightDown;
        float _timeTotal, _timeAscend, _timeHeightDiff;

        direction = target.position - transform.position;
        _deltaHeightDown = Mathf.Abs(direction.y);
        direction.y = transform.position.y + height;

        heightInitial = transform.position.y;

        velocityYInitial = Mathf.Sqrt(2 * Global.g * height);
        velocityYInitialSquared = velocityYInitial * velocityYInitial;

        _timeAscend = velocityYInitial / Global.g;
        _timeHeightDiff = (Mathf.Sqrt(2 * Global.g * _deltaHeightDown + velocityYInitialSquared) - velocityYInitial) / Global.g;
        _timeTotal = 2 * _timeAscend + _timeHeightDiff;

        velocity.x = direction.x / _timeTotal;
        velocity.z = direction.z / _timeTotal;
    }

    private void Update()
    {
        if (reachedTarget)
        {
            CheckTimeElapsedSinceExplosionStart();
            return;
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        deltaHeightUp = transform.position.y - heightInitial;

        // Above given height so it should start falling.
        if (deltaHeightUp >= height)
        {
            ascending = -1;
            deltaHeightUp = height - Global.eps;
        }

        velocity.y = Mathf.Sqrt(velocityYInitialSquared - 2 * Global.g * deltaHeightUp);

        Vector3 currentDirection = velocity;
        currentDirection.y = ascending * velocity.y;

        transform.position += currentDirection * Time.deltaTime;
    }

    /// <summary>
    /// Updates the time elapsed since explosion start. When it is above VFX total time destroy current object.
    /// </summary>
    private void CheckTimeElapsedSinceExplosionStart()
    {
        timeElapsedSinceExplosion += Time.deltaTime;
        
        if (timeElapsedSinceExplosion >= VFXtime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Floor"))
            return;

        reachedTarget = true;
        meshRenderer.enabled = false;

        float scale = 18f;
        explosionTransform.localScale = new Vector3(scale, scale, scale);

        VFX.Play();
    }
}
