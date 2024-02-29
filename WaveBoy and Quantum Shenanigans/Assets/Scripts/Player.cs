using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }
    public enum HitPointType
    {
        ElectronHP,
        PositronHP
    }

    public event EventHandler OnChangeInHP;


    private int ElectronHP = 100;
    private int PositronHP = 100;
    private float moveSpeed = 5f;
    private int maxHP = 100;
    private bool canShoot = true;
    private float firerate = 2f;
    private PlayerInput playerInput = null;
    private Rigidbody2D rb = null;
    private float Timer;
    private Vector2 aim;
    private float aimingAngle;
    private GameManager.IsWhat isWhat;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        Timer = Time.time;
    }

    private void Start()
    {
        isWhat = GameManager.IsWhat.Player;
    }


    public void DamageControl(GameManager.IsWhat isWhat, int take, int amount)
    {
        if (take == 1)
        {
            if (isWhat == GameManager.IsWhat.Electron)
            {
                ElectronHP += take * amount;
            }
            else if (isWhat == GameManager.IsWhat.Positron)
            {
                PositronHP += take * amount;
            }
            else
            {
                ElectronHP += take * amount;
                PositronHP += take * amount;
            }
        }
        else
        {
            if (isWhat == GameManager.IsWhat.Electron)
            {
                PositronHP += take * amount;
            }
            else if (isWhat == GameManager.IsWhat.Positron)
            {
                ElectronHP += take * amount;
            }
            else
            {
                ElectronHP += take * amount;
                PositronHP += take * amount;
            }
        }

        if (ElectronHP > maxHP)
        {
            ElectronHP = maxHP;
        }
        if (PositronHP > maxHP)
        {
            PositronHP = maxHP;
        }

        OnChangeInHP?.Invoke(this, EventArgs.Empty);
    }

    private void OnMove(InputValue value)
    {
        rb.velocity = value.Get<Vector2>() * moveSpeed;
    }

    private void OnAim(InputValue value)
    {
        if (value.Get<Vector2>() != Vector2.zero)
        {
            aim = value.Get<Vector2>();
            if (aim[0] + aim[1] > 2.1f)
            {
                aim = Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - transform.position;
            }
            aimingAngle = Mathf.Atan2(aim[1], aim[0]) * Mathf.Rad2Deg - 90f;

        }
    }

    private void OnFire1(InputValue value)
    {
        if (Timer < Time.time)
        {
            shoot(GameManager.IsWhat.Electron, aim);
        }
    }

    private void OnFire2(InputValue value)
    {
        if (Timer < Time.time)
        {
            shoot(GameManager.IsWhat.Positron, aim);
        }
    }

    private void OnFire3(InputValue value)
    {
        if (Timer < Time.time && GameManager.instance.difficulty > 0)
        {
            shoot(GameManager.IsWhat.Photon_up, aim);
        }
    }

    private void OnFire4(InputValue value)
    {
        if (Timer < Time.time && GameManager.instance.difficulty > 1)
        {
            shoot(GameManager.IsWhat.Photon_down, aim);
        }
    }



    private void shoot(GameManager.IsWhat isWhat, Vector2 dir)
    {
        Timer = Time.time + 1 / firerate;
        dir.Normalize();
        Particle.SpawnParticle(GameManager.GetParticleSOFromIsWhat(isWhat), (Vector2)transform.position + 3 * (Vector2)dir / 2, dir, false);
        switch (isWhat)
        {
            case GameManager.IsWhat.Photon_up:
                DamageControl(isWhat, -1, 5);
                break;
            case GameManager.IsWhat.Photon_down:
                DamageControl(isWhat, -1, 5);
                break;
            case GameManager.IsWhat.Electron:
                Debug.Log(isWhat);
                DamageControl(GameManager.IsWhat.Positron, -1, 5);
                break;
            case GameManager.IsWhat.Positron:
                DamageControl(GameManager.IsWhat.Electron, -1, 5);
                break;
            case GameManager.IsWhat.Player:
                break;
        }
    }

    public float GetHitPointValueNormalized(HitPointType hitPointType)
    {
        return (float)((hitPointType == HitPointType.ElectronHP) ? ElectronHP : PositronHP) / maxHP;
    }

    public void ResetTimer()
    {
        Timer = 0f;

    }

    public float GetAimingAngle()
    {
        return aimingAngle;
    }

}
