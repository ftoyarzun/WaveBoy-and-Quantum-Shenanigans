using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particle : MonoBehaviour
{

    public static event EventHandler<OnParticleCollisionEventAgrs> OnParticleCollision;
    public class OnParticleCollisionEventAgrs: EventArgs
    {
        public GameManager.IsWhat isWhat;
    }

    protected Vector2 dir;
    protected Vector2 vel;
    protected Rigidbody2D Rb;
    protected float speed = 6f;
    protected bool isEnemy = true;

    [SerializeField] protected GameManager.IsWhat isWhat;
    [SerializeField] protected ParticleSOList particleSOList;

    protected float timerToSetAsEnemy = 1f;
    protected float smooth = 100f;
    protected float lightVelocity = 5f;


    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isEnemy && (isWhat == GameManager.IsWhat.Electron || isWhat == GameManager.IsWhat.Positron))
        {
            dir = -(transform.position - Player.Instance.transform.position);

            dir.Normalize();
            vel = Rb.velocity;
            vel.Normalize();

        }

        if (isEnemy && !(isWhat == GameManager.IsWhat.Electron || isWhat == GameManager.IsWhat.Positron))
        {
            if (Rb.velocity.magnitude > 0.01f)
            {
                Rb.velocity = Rb.velocity / Rb.velocity.magnitude * lightVelocity;
            }
        }

        float angle = Mathf.Atan2(Rb.velocity[1], Rb.velocity[0]) * Mathf.Rad2Deg;
        Rb.rotation = angle;

        if (!isEnemy)
        {
            timerToSetAsEnemy -= Time.deltaTime;
            if (timerToSetAsEnemy <= 0)
            {
                isEnemy = true;
            }
        }

        if (transform.position.magnitude > 30)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isEnemy && (isWhat == GameManager.IsWhat.Electron || isWhat == GameManager.IsWhat.Positron))
        {
            Rb.velocity = Vector3.Lerp(vel, dir, smooth * Time.deltaTime) * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.TryGetComponent<Particle>(out Particle collidedParticle))
        {
            if (isEnemy && !collidedParticle.IsEnemy() || !isEnemy && !collidedParticle.IsEnemy())
            {
                if ((isWhat == GameManager.IsWhat.Electron && collidedParticle.GetIsWhat() == GameManager.IsWhat.Positron)
                    || (isWhat == GameManager.IsWhat.Positron && collidedParticle.GetIsWhat() == GameManager.IsWhat.Electron))
                {
                    Vector2 norm_dir = Vector3.Cross(Vector3.back, dir).normalized;
                    Destroy(this.gameObject);
                    Destroy(collision.gameObject);
                    Player.Instance.DamageControl(isWhat, 1, 10);
                    Player.Instance.ResetTimer();
                    GameManager.Instance.UpdateHighscore();
                    GameManager.Instance.SpawnPhotons(transform.position, norm_dir);
                    OnParticleCollision?.Invoke(this, new OnParticleCollisionEventAgrs {isWhat = isWhat});
                }
                else if ((isWhat == collidedParticle.GetIsWhat()) && (isWhat == GameManager.IsWhat.Photon_up || isWhat == GameManager.IsWhat.Photon_down))
                {
                    Destroy(this.gameObject);
                    Destroy(collision.gameObject);
                    Player.Instance.DamageControl(isWhat, 1, 10);
                    Player.Instance.ResetTimer();
                    GameManager.Instance.UpdateHighscore();
                    OnParticleCollision?.Invoke(this, new OnParticleCollisionEventAgrs { isWhat = isWhat });
                }
                else
                {
                    //speed *= 2;
                }
            }
            else if (isEnemy && collidedParticle.IsEnemy())
            {
                if ((isWhat == collidedParticle.GetIsWhat()) && (isWhat == GameManager.IsWhat.Photon_up || isWhat == GameManager.IsWhat.Photon_down))
                {
                    Destroy(this.gameObject);
                    Destroy(collision.gameObject);
                    GameManager.Instance.UpdateHighscore();
                    OnParticleCollision?.Invoke(this, new OnParticleCollisionEventAgrs { isWhat = isWhat });
                }
            }
            else
            {

            }
        }


        if (collision.gameObject.TryGetComponent<Player>(out Player collidedPlayer))
        {
            Destroy(this.gameObject);
            collidedPlayer.DamageControl(isWhat, -1, 10);
            GameManager.Instance.ResetCombo();
        }
    }

    public void SetTimerTo(float timer)
    {
        timerToSetAsEnemy = timer;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public GameManager.IsWhat GetIsWhat()
    {
        return isWhat;
    }

    public void Starter(bool IsEnemy, float Speed, GameManager.IsWhat IsWhat)
    {
        isEnemy = IsEnemy;
    }

    public static void SpawnParticle(ParticleSO particleSO, Vector2 position, Vector2 direction, bool isEnemy, float force = 400)
    {
        Particle particle = Instantiate(particleSO.prefab, position, Quaternion.identity, ParticleManager.Instance.transform);
        particle.Starter(isEnemy, 0, particleSO.isWhat);
        particle.GetComponent<Rigidbody2D>().AddForce(direction * force);
    }
}