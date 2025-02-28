using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    private float _height;

    private float _deltaHeightUp, _heightInitial;
    private Vector3 _velocity;
    private float _velocityYInitial, _velocityYInitialSquared;

    private int _ascending = 1;
    private bool _reachedTarget = false;
    private MeshRenderer _renderer;

    private ParticleSystem _VFX;
    private float _timeElapsedSinceExplosion = 0f;
    private float _VFXtime;

    private Transform _explosionTransform;
    private Explosion _explosion;

    public float Height
    {
        set { _height = value; }
    }

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _VFX = GetComponentInChildren<ParticleSystem>();
        _explosionTransform = _VFX.transform.Find("Hitbox");
        _explosion = _explosionTransform.gameObject.GetComponent<Explosion>();

        _VFX.Stop();
        _VFXtime = _VFX.main.duration + _VFX.main.startLifetime.constant;

        _explosion.Damage = _damage;
        _explosion.Element = _element;

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

        _direction = _target.position - transform.position;
        _deltaHeightDown = Mathf.Abs(_direction.y);
        _direction.y = transform.position.y + _height;

        _heightInitial = transform.position.y;

        _velocityYInitial = Mathf.Sqrt(2 * Global.g * _height);
        _velocityYInitialSquared = _velocityYInitial * _velocityYInitial;

        _timeAscend = _velocityYInitial / Global.g;
        _timeHeightDiff = (Mathf.Sqrt(2 * Global.g * _deltaHeightDown + _velocityYInitialSquared) - _velocityYInitial) / Global.g;
        _timeTotal = 2 * _timeAscend + _timeHeightDiff;

        _velocity.x = _direction.x / _timeTotal;
        _velocity.z = _direction.z / _timeTotal;
    }

    private void Update()
    {
        if (_reachedTarget)
        {
            CheckTimeElapsedSinceExplosionStart();
            return;
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        _deltaHeightUp = transform.position.y - _heightInitial;

        // Above given height so it should start falling.
        if (_deltaHeightUp >= _height)
        {
            _ascending = -1;
            _deltaHeightUp = _height - Global.eps;
        }

        _velocity.y = Mathf.Sqrt(_velocityYInitialSquared - 2 * Global.g * _deltaHeightUp);

        Vector3 currentDirection = _velocity;
        currentDirection.y = _ascending * _velocity.y;

        transform.position += currentDirection * Time.deltaTime;
    }

    /// <summary>
    /// Updates the time elapsed since explosion start. When it is above VFX total time destroy current object.
    /// </summary>
    private void CheckTimeElapsedSinceExplosionStart()
    {
        _timeElapsedSinceExplosion += Time.deltaTime;
        
        if (_timeElapsedSinceExplosion >= _VFXtime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Floor"))
            return;

        _reachedTarget = true;
        _renderer.enabled = false;

        float scale = 18f;
        _explosionTransform.localScale = new Vector3(scale, scale, scale);

        _VFX.Play();
    }
}
