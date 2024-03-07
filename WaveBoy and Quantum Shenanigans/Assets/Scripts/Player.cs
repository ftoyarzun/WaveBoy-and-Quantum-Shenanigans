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
    private int maxHP = 100;

    private float moveSpeed = 8f;
    private float firerate = 2f;

    private float Timer;
    private float aimingAngle;

    private float pushTimer;
    private float pushTimerMax = 0.5f;

    private PlayerInput playerInput = null;
    private Rigidbody2D rb = null;
    private GameManager.IsWhat isWhat;

    private Vector2 moveDir;
    private Vector2 aim;

    private bool gotPushed = false;

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

    private void Update()
    {
        if (!gotPushed)
        {
            rb.velocity = moveDir;
        }
        else
        {
            pushTimer += Time.deltaTime;
        }

        if (pushTimer > pushTimerMax)
        {
            pushTimer = 0;
            gotPushed = false;
        }

        if (transform.position.magnitude > 16.31)
        {
            transform.position = Vector3.zero;
        }
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
        moveDir = value.Get<Vector2>().normalized * moveSpeed;
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
            shoot(GameManager.IsWhat.Photon_up, aim);
        }
    }

    private void OnFire2(InputValue value)
    {
        if (Timer < Time.time)
        {
            shoot(GameManager.IsWhat.Photon_down, aim);
        }
    }

    private void OnFire3(InputValue value)
    {
        if (Timer < Time.time)
        {
            shoot(GameManager.IsWhat.Electron, aim);
        }
    }

    private void OnFire4(InputValue value)
    {
        if (Timer < Time.time)
        {
            shoot(GameManager.IsWhat.Positron, aim);
        }
    }



    private void shoot(GameManager.IsWhat isWhat, Vector2 dir)
    {
        switch (GameManager.Instance.GetPlayingState())
        {
            case GameManager.PlayingState.Fase1:
                return;
            case GameManager.PlayingState.Fase2:
                if ((isWhat == GameManager.IsWhat.Electron) || (isWhat == GameManager.IsWhat.Positron)) return;
                break;
            case GameManager.PlayingState.Fase3:
                if ((isWhat == GameManager.IsWhat.Electron) || (isWhat == GameManager.IsWhat.Positron)) return;
                break;
            case GameManager.PlayingState.Fase4:
                if ((isWhat == GameManager.IsWhat.Electron) || (isWhat == GameManager.IsWhat.Positron)) return;
                break;
            case GameManager.PlayingState.Fase5:
                break;
            case GameManager.PlayingState.Fase6:
                break;
            case GameManager.PlayingState.Boss1:
                break;
            case GameManager.PlayingState.Boss2:
                break;
        }

        Timer = Time.time + 1 / firerate;
        dir.Normalize();
        Particle.SpawnParticle(GameManager.GetParticleSOFromIsWhat(isWhat), (Vector2)transform.position + 3 * (Vector2)dir / 2, dir, false, 800f);
        switch (isWhat)
        {
            case GameManager.IsWhat.Photon_up:
                DamageControl(isWhat, -1, 5);
                break;
            case GameManager.IsWhat.Photon_down:
                DamageControl(isWhat, -1, 5);
                break;
            case GameManager.IsWhat.Electron:
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

    public void ResetPlayerHitPoints()
    {
        ElectronHP = maxHP;
        PositronHP = maxHP;
        OnChangeInHP?.Invoke(this, EventArgs.Empty);
    }

    public void GetPushed()
    {
        gotPushed = true;
        pushTimer = 0;
    }
}
