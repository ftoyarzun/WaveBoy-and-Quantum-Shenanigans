using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Boss : MonoBehaviour
{
    public enum Behavior
    {
        Att1,
        Att2,
        Att3,
        Att4,
        Start
    }

    public event EventHandler OnBehaviorChanged;
    public event EventHandler OnHitPointChanged;

    private float colliderTimer;
    private float colliderTimerMax = 0.5f;

    private float behaviorChangeTimer;
    private float behaviorChangeTimerMax = 5f;

    private float attackTimer;
    private float attackTimerMax = 2;

    private float attackFirerateTimer;
    private float fireRate;

    private float scale;
    private float steps;

    private bool isEnemy = true;

    private GameManager.IsWhat isWhat;
    private Behavior behavior;

    private Vector2 dir;
    private Vector2 shootDirection;
    private Vector2 vectorToBase;

    private int bossHP = 10;
    private int bossHPMax = 50;

    new Rigidbody2D rigidbody2D;

    private Transform BossBasePosition;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        scale = transform.localScale.x;
    }
    private void Start()
    {
        isWhat = GameManager.IsWhat.Electron;
        behavior = Behavior.Start;
        bossHP = bossHPMax;
    }

    private void Update()
    {
        colliderTimer += Time.deltaTime;
        behaviorChangeTimer += Time.deltaTime;
        if (behaviorChangeTimer > behaviorChangeTimerMax && behavior != Behavior.Start)
        {
            behaviorChangeTimer = 0f;
            behavior = (Behavior) UnityEngine.Random.Range(0, 4);
            isWhat = (GameManager.IsWhat)UnityEngine.Random.Range(0, 4);
            OnBehaviorChanged?.Invoke(this, EventArgs.Empty);

        }
        switch (behavior)
        {
            case Behavior.Att1:
                Attack1();
                break;
            case Behavior.Att2:
                Attack2();
                break;
            case Behavior.Att3:
                break;
            case Behavior.Att4:
                break;
            case Behavior.Start:
                StartingBehavior();
                break;

        }

        if (bossHP <= 0)
        {
            Destroy(gameObject); 
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
                    Destroy(collision.gameObject);
                    Player.Instance.DamageControl(isWhat, 1, 10);
                    //Player.Instance.ResetTimer();
                    GameManager.Instance.SpawnPhotons(transform.position, scale/2);
                    bossHP--;
                    OnHitPointChanged?.Invoke(this,EventArgs.Empty);
                }
                else if ((isWhat == collidedParticle.GetIsWhat()) && (isWhat == GameManager.IsWhat.Photon_up || isWhat == GameManager.IsWhat.Photon_down))
                {
                    Destroy(collision.gameObject);
                    Player.Instance.DamageControl(isWhat, 1, 10);
                    //Player.Instance.ResetTimer();
                    GameManager.Instance.UpdateHighscore();
                    bossHP--;
                    OnHitPointChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    bossHP = Mathf.Clamp(bossHP+1, 0, bossHPMax);
                    Destroy(collision.gameObject);
                    OnHitPointChanged?.Invoke(this, EventArgs.Empty);
                    //speed *= 2;
                }
            }
            else if (isEnemy && collidedParticle.IsEnemy())
            {
                if ((isWhat == collidedParticle.GetIsWhat()) && (isWhat == GameManager.IsWhat.Photon_up || isWhat == GameManager.IsWhat.Photon_down))
                {
                    Destroy(collision.gameObject);
                    GameManager.Instance.UpdateHighscore();
                }
            }
            else
            {
            }
        }


        if (collision.gameObject.TryGetComponent<Player>(out Player collidedPlayer))
        {
            if (colliderTimer > colliderTimerMax)
            {
                colliderTimer = 0;
                collidedPlayer.DamageControl(isWhat, -1, 30);
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 pushDirection = transform.position - Player.Instance.transform.position;
                pushDirection.Normalize();
                pushDirection *= 20;
                Player.Instance.GetPushed();
                playerRB.AddForce(pushDirection, ForceMode2D.Impulse);
                GameManager.Instance.ResetCombo();
            }
            
        }
    }


    private void Attack1()
    {
        MoveTowardsBase();
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTimerMax) 
        {
            attackTimer = 0;
            shootDirection = new Vector2(-transform.position.x, 0).normalized;
            float angleSpan = 111f;
            float particlesToShoot = 8f;
            for (int i = 0; i < particlesToShoot; i++)
            {
                Vector2 shootParticleDirection = Quaternion.Euler(0, 0, angleSpan / particlesToShoot * (i - particlesToShoot / 2f)) * shootDirection;
                GameManager.IsWhat particleIsWhat = (GameManager.IsWhat)UnityEngine.Random.Range(0,2);
                Particle.SpawnParticle(GameManager.GetParticleSOFromIsWhat(particleIsWhat), (Vector2)this.transform.position + shootParticleDirection * 8, shootParticleDirection, true, 10);
            }
        }
    }

    private void Attack2()
    {
        if (rigidbody2D.velocity.magnitude < 0.01)
        {
            rigidbody2D.velocity = new Vector2(-transform.position.x, transform.position.x).normalized * 5;
        }
    }

    private void Attack3()
    {

    }


    private void StartingBehavior()
    {
        steps = Time.deltaTime * 3;
        vectorToBase = transform.position - BossBasePosition.position;
        if (vectorToBase.magnitude > 2 * steps)
        {
            transform.position = Vector3.MoveTowards(transform.position, BossBasePosition.position, steps);
        }
        else if (behavior == Behavior.Start)
        {
            behavior = (Behavior)UnityEngine.Random.Range(0, 4);
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.mass = 1000000;
        }
    }

    public GameManager.IsWhat GetIsWhat()
    {
        return isWhat;
    }

    public float GetCurrentHPNormalized()
    {
        return (float)bossHP / bossHPMax;
    }

    private void MoveTowardsBase()
    {
        rigidbody2D.velocity = Vector2.zero;
        steps = Time.deltaTime * 3;
        vectorToBase = transform.position - BossBasePosition.position;
        if (vectorToBase.magnitude > 2 * steps)
        {
            transform.position = Vector3.MoveTowards(transform.position, BossBasePosition.position, steps);
        }
    }

    public void SetBasePosition(Transform baseTransform)
    {
        BossBasePosition = baseTransform;
    }
}
