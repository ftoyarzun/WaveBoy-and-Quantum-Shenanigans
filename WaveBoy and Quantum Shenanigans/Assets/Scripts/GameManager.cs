using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;
using System;
using Unity.Mathematics;
using static GameManager;

public class GameManager : MonoBehaviour
{

    //Functions that use IsWhat with switch 
    //Player.Shoot
    public enum IsWhat
    {
        Photon_up,
        Photon_down,
        Electron,
        Positron,
        Player
    }

    public enum GameState
    {
        Pause,
        Playing,
        GameOver
    }

    //Functions that use the PlayingState with switch 
    //GameManager.Update
    //Player.Shoot
    //UIAssistant.TextWriter_OnTriggerString
    //UIAssistant.Update
    //ScientistVisuals.GameManager_OnPlayingStateChanged
    public enum PlayingState
    {
        Fase1,
        Fase2,
        Fase3,
        Fase4,
        Fase5,
        Fase6,
        Boss1,
        Boss2,
        Died
    }

    public static GameManager Instance { get; private set; }

    public event EventHandler OnPlayingStateChanged;
    public event EventHandler OnGamePaused;



    private GameState gameState;

    private PlayingState playingState = PlayingState.Fase1;
    private PlayingState lastPlayingState = PlayingState.Fase1;

    private static bool gameIsPaused;
    private bool faseStarting = true;
    private bool bossfaseStarting = true;
    private bool diedStarting = true;
    private bool bossfase2Starting = true;


    public int highscore = 0;
    public int combo = 0;
    public int MaxCombo = 0;

    private float randomAngle;
    private int angleSpanForRandomAngle = 20;

    private float timerToSpawnParticles;
    private float Spawnrate = 2;
    private float laserAngle;

    private float fase1Timer;
    private float fase2Timer;
    private float fase3Timer;
    private float fase4Timer;
    private float fase5Timer;
    private float fase6Timer;
    private float boss1Timer;
    private float boss2Timer;
    private float diedTimer;

    private float fase1TimerMax = 2f;
    private float fase2TimerMax = 3f;
    private float fase3TimerMax = 10f;
    private float fase4TimerMax = 10f;
    private float fase5TimerMax = 20f;
    private float fase6TimerMax = 20f;
    private float boss2TimerMax = 2f;
    private float diedTimerMax = 2;

    private float fase3TimerMax_1 = 10;
    private float fase3TimerMax_2 = 20;
    private float fase3TimerMax_3 = 30;

    private float fase4TimerMax_1 = 10f;
    private float fase4TimerMax_2 = 20f;
    private float fase4TimerMax_3 = 30f;

    private float fase6TimerMax_1 = 10f;
    private float fase6TimerMax_2 = 20f;
    private float fase6TimerMax_3 = 30f;

    private Boss boss;



    [SerializeField] private ParticleSOList particleSOList;
    [SerializeField] private TMP_Text Highscore;
    [SerializeField] private TMP_Text Combo;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private GameObject bossSpawnPosition;
    [SerializeField] private Transform bossBasePosition;


    public Vector2 randomSpawnPosition;
    public Vector2 aimAtPlayer;
    


    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetCombo();
        Player.Instance.OnPlayerDied += Player_OnPlayerDied;
    }

    private void Player_OnPlayerDied(object sender, EventArgs e)
    {
        playingState = PlayingState.Died;
        OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        switch (playingState)
        {
            case GameManager.PlayingState.Fase1:
                GameManagerFase1();
                break;
            case GameManager.PlayingState.Fase2:
                GameManagerFase2();
                break;
            case GameManager.PlayingState.Fase3:
                GameManagerFase3();
                break;
            case GameManager.PlayingState.Fase4:
                GameManagerFase4();
                break;
            case GameManager.PlayingState.Fase5:
                GameManagerFase5();
                break;
            case GameManager.PlayingState.Fase6:
                GameManagerFase6();
                break;
            case GameManager.PlayingState.Boss1:
                GameManagerBoss1();
                break;
            case GameManager.PlayingState.Boss2:
                GameManagerBoss2();
                break;
            case GameManager.PlayingState.Died:
                GameManagerDied();
                break;
        }
    }

    public void makeClone(int x, int y)
    {
        if (UnityEngine.Random.Range(0, 10) > 1)
        {
            randomSpawnPosition = new Vector2(x * UnityEngine.Random.Range(8, 12), y * UnityEngine.Random.Range(8, 12));
            aimAtPlayer = -(randomSpawnPosition - (Vector2) Player.Instance.transform.position);
            Particle.SpawnParticle(particleSOList.particleSOList[UnityEngine.Random.Range(0,4)], randomSpawnPosition, aimAtPlayer, true, 10f);
        }
    }

    private void makeClone(float x, float y, GameManager.IsWhat isWhat, float spawnProbability)
    {
        if (UnityEngine.Random.Range(0, 10) > (1f - spawnProbability) *10f)
        {
            Vector2 spawnPosition = new Vector2(x,y);
            aimAtPlayer = -(spawnPosition - (Vector2)Player.Instance.transform.position);
            randomAngle = UnityEngine.Random.Range(-angleSpanForRandomAngle, angleSpanForRandomAngle);
            aimAtPlayer = Quaternion.Euler(0f, 0f, randomAngle) * aimAtPlayer;
            aimAtPlayer.Normalize();
            Particle.SpawnParticle(GetParticleSOFromIsWhat(isWhat), spawnPosition, aimAtPlayer, true, 10f);
        }
    }

    private void makeClone(float x, float y, GameManager.IsWhat isWhat, Vector2 direction)
    {

        Vector2 spawnPosition = new Vector2(x, y);
        Particle.SpawnParticle(GetParticleSOFromIsWhat(isWhat), spawnPosition, direction, true, 10f);

    }

    public void UpdateHighscore()
    {
        highscore += 1;
        Highscore.text = "HIGHSCORE: " + highscore;
        combo += 1;
        if (combo > MaxCombo)
        {
            MaxCombo = combo;
        }
        Combo.text = "Current COMBO: " + combo + "\nMAX COMBO: " + MaxCombo;
    }

    public void ResetCombo()
    {
        combo = 0;
        Combo.text = "Current COMBO:" + combo + "\nMAX COMBO" + MaxCombo;
    }

    public void TogglePauseGame()
    {
        Time.timeScale = 1f - Time.timeScale;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void SpawnPhotons(Vector2 pos, Vector2 vel)
    {
        vel.Normalize();

        makeClone(pos.x + vel.x, pos.y + vel.y, GameManager.IsWhat.Photon_up, vel);
        makeClone(pos.x - vel.x, pos.y - vel.y, GameManager.IsWhat.Photon_down, -vel);
    }

    public void SpawnPhotons(Vector2 pos, float mag)
    {
        float randomAngle = UnityEngine.Random.Range(0, 180);
        Vector2 direction = Quaternion.Euler(0,0,randomAngle) * new Vector2(mag, 0);

        makeClone(pos.x + direction.x, pos.y + direction.y, GameManager.IsWhat.Photon_up, direction);
        makeClone(pos.x - direction.x, pos.y - direction.y, GameManager.IsWhat.Photon_down, -direction);
    }


    public static ParticleSO GetParticleSOFromIsWhat(GameManager.IsWhat isWhat)
    {
        foreach (ParticleSO particleSO in Instance.particleSOList.particleSOList)
        {
            if (isWhat == particleSO.isWhat)
            {
                return particleSO;
            }
        }
        return null;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public PlayingState GetPlayingState()
    {
        return playingState;
    }

    private void GameManagerFase1()
    {
        if (faseStarting)
        {
            Player.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
            lastPlayingState = playingState;
        }
        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            fase1Timer += Time.deltaTime;
            if (fase1Timer > fase1TimerMax)
            {
                playingState = PlayingState.Fase2;
                faseStarting = true;
                OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);   
            }
        }
        
    }

    private void GameManagerFase2()
    {
        if (faseStarting)
        {
            Player.Instance.Show();
            PlayerHitPointManager.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
            lastPlayingState = playingState;
            fase2Timer = 0;
        }
        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            fase2Timer += Time.deltaTime;
            if (fase2Timer > fase2TimerMax)
            {
                playingState = PlayingState.Fase3;
                faseStarting = true;
                OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GameManagerFase3()
    {
        if (faseStarting)
        {
            Player.Instance.Show();
            PlayerHitPointManager.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
            lastPlayingState = playingState;
            fase3Timer = 0;
        }
        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            fase3Timer += Time.deltaTime;
            if (timerToSpawnParticles < Time.time)
            {
                float randomX = UnityEngine.Random.Range(-14f, 14f);
                float randomY = UnityEngine.Random.Range(-8f, 8f);
                if ((fase3Timer > fase3TimerMax_3) && ParticleManager.Instance.GetNumberOfParticles() == 0)
                {
                    playingState = PlayingState.Fase4;
                    faseStarting = true;
                    OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
                }

                else if ((fase3Timer > fase3TimerMax_3))
                {

                }

                else if (fase3Timer > fase3TimerMax_2)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        makeClone(randomX, -10, GameManager.IsWhat.Photon_up, 0.9f);
                        makeClone(randomX, 10, GameManager.IsWhat.Photon_down, 0.9f);
                    }
                    else
                    {
                        makeClone(randomX, 10, GameManager.IsWhat.Photon_up, 0.9f);
                        makeClone(randomX, -10, GameManager.IsWhat.Photon_down, 0.9f);
                    }
                }

                else if (fase3Timer > fase3TimerMax_1)
                {
                    makeClone(randomX, 10, GameManager.IsWhat.Photon_up, 0.9f);
                    makeClone(randomX, -10, GameManager.IsWhat.Photon_down, 0.9f);
                }

                else
                {
                    makeClone(randomX, -10, GameManager.IsWhat.Photon_up, 0.9f);
                    makeClone(randomX, 10, GameManager.IsWhat.Photon_down, 0.9f);
                }


                timerToSpawnParticles = Time.time + 1 / Spawnrate * 2;
            }  
        }
        
    }

    private void GameManagerFase4()
    {
        if (faseStarting)
        {
            Player.Instance.Show();
            PlayerHitPointManager.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
            lastPlayingState = playingState;
            fase4Timer = 0;
        }
        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            fase4Timer += Time.deltaTime;
            if ((fase4Timer > fase4TimerMax_3) && ParticleManager.Instance.GetNumberOfParticles() == 0)
            {
                playingState = PlayingState.Fase5;
                faseStarting = true;
                OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
            }

            else if ((fase4Timer > fase4TimerMax_3))
            {

            }

            else if (fase4Timer > fase4TimerMax_2)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    float angleSpeed = 60;
                    laserAngle += Time.deltaTime * angleSpeed;
                    SpawnLaser(true, true, laserAngle);
                }
                else
                {
                    float angleSpeed = 40;
                    laserAngle -= Time.deltaTime * angleSpeed;
                    SpawnLaser(true, true, laserAngle);
                }
            }

            else if (fase4Timer > fase4TimerMax_1)
            {
                float angleSpeed = 20;
                laserAngle += Time.deltaTime * angleSpeed;
                SpawnLaser(true, true, laserAngle);
            }

            else
            {
                SpawnLaser(true, true, 0f);
            }
        }
    }

    private void GameManagerFase5()
    {
        if (faseStarting)
        {
            Player.Instance.Show();
            PlayerHitPointManager.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
            lastPlayingState = playingState;
            fase5Timer = 0;
        }

        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            fase5Timer += Time.deltaTime;
            if (timerToSpawnParticles < Time.time)
            {
                if ((fase5Timer > fase5TimerMax) && ParticleManager.Instance.GetNumberOfParticles() == 0)
                {
                    playingState = PlayingState.Fase6;
                    faseStarting = true;
                    OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
                }

                else if (fase5Timer > fase5TimerMax)
                {

                }
                else
                {
                    float randomX = UnityEngine.Random.Range(-14f, 14f);
                    makeClone(randomX, 10, GameManager.IsWhat.Electron, 0.6f);
                }


                timerToSpawnParticles = Time.time + 1 / Spawnrate * 2;
            }

        }
    }

    private void GameManagerFase6()
    {
        if (faseStarting)
        {
            Player.Instance.Show();
            PlayerHitPointManager.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
            lastPlayingState = playingState;
            fase6Timer = 0;
        }
        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            fase6Timer += Time.deltaTime;
            if (timerToSpawnParticles < Time.time)
            {
                float randomX = UnityEngine.Random.Range(-14f, 14f);
                float randomY = UnityEngine.Random.Range(-8f, 8f);
                if ((fase6Timer > fase6TimerMax_3) && ParticleManager.Instance.GetNumberOfParticles() == 0)
                {
                    playingState = PlayingState.Boss1;
                    faseStarting = true;
                    OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
                }

                else if ((fase6Timer > fase6TimerMax_3))
                {

                }

                else if (fase6Timer > fase6TimerMax_2)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        makeClone(randomX, -10, GameManager.IsWhat.Electron, 0.3f);
                        makeClone(randomX, 10, GameManager.IsWhat.Positron, 0.3f);
                    }
                    else
                    {
                        makeClone(randomX, 10, GameManager.IsWhat.Electron, 0.3f);
                        makeClone(randomX, -10, GameManager.IsWhat.Positron, 0.3f);
                    }
                }

                else if (fase6Timer > fase6TimerMax_1)
                {
                    makeClone(randomX, 10, GameManager.IsWhat.Electron, 0.3f);
                    makeClone(randomX, -10, GameManager.IsWhat.Positron, 0.3f);
                }

                else
                {
                    makeClone(randomX, -10, GameManager.IsWhat.Electron, 0.3f);
                    makeClone(randomX, 10, GameManager.IsWhat.Positron, 0.3f);
                }


                timerToSpawnParticles = Time.time + 1 / Spawnrate * 2;
            }
        }

    }

    private void GameManagerBoss1()
    {
        if (faseStarting)
        {
            BossHPUI.Instance.Hide();
            Player.Instance.Show();
            PlayerHitPointManager.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
            bossfaseStarting = true;
            lastPlayingState = playingState;
        }
        else if (bossfaseStarting && !UIAssistant.Instance.GetHasToDispayText())
        {
            bossfaseStarting = false;
            boss = Instantiate(bossPrefab, bossSpawnPosition.transform.position, Quaternion.identity);
            boss.SetBasePosition(bossBasePosition);
            BossHPUI.Instance.SetBoss(boss);
            BossHPUI.Instance.Show();
        }
        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            if (boss.GetCurrentHPNormalized() <= 0f)
            {
                playingState = PlayingState.Boss2;
                faseStarting = true;
                OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GameManagerBoss2()
    {
        if (faseStarting)
        {
            BossHPUI.Instance.Hide();
            Player.Instance.Show();
            PlayerHitPointManager.Instance.Hide();
            Player.Instance.ResetPlayerHitPoints();
            UIAssistant.Instance.SetHasToDispayText();
            faseStarting = false;
            bossfaseStarting = true;
            lastPlayingState = playingState;
            boss2Timer = 0;
        }

        else if (boss2Timer < boss2TimerMax)
        {
            boss2Timer += Time.deltaTime;
        }

        else if (boss2Timer > boss2TimerMax)
        {
            CutsceneManager.Instance.Show();
        }

        else if (!UIAssistant.Instance.GetHasToDispayText())
        {
            Debug.Log("Finished");
        }

    }

    private void GameManagerDied()
    {
        if (faseStarting)
        {
            PlayerHitPointManager.Instance.Hide();
            Player.Instance.Hide();
            Player.Instance.ResetPlayerHitPoints();
            faseStarting = false;
        }
        else if (diedTimer < diedTimerMax)
        {
            diedTimer += Time.deltaTime;
        }
        else if (diedStarting)
        {
            diedStarting = false;
            ParticleManager.Instance.DestroyParticles();
            if (boss != null) { Destroy(boss.gameObject); }
            CutsceneManager.Instance.Show();
            UIAssistant.Instance.SetHasToDispayText();

        }

        else if (!CutsceneManager.Instance.IsPlaying())
        {
            DiedScreenUI.Instance.Show();
            //CutsceneManager.Instance.Hide();
        }
    }



    private void StartingFase()
    {
        Player.Instance.Show();
        UIAssistant.Instance.SetHasToDispayText();
        Player.Instance.ResetPlayerHitPoints();
        faseStarting = false;
        lastPlayingState = playingState;
    }

    private void SpawnLaser(bool right, bool up, float angle)
    {
        Vector2 startDirection = Vector2.zero;
        startDirection.x = right? 1 : -1;
        startDirection.y = up ? 1 : -1;

        startDirection = Quaternion.Euler(0f,0f,angle) * startDirection;

        Vector2 spreadDireciton = new Vector2(-startDirection.y, startDirection.x);

        float laserSpread = 7f;
        if (timerToSpawnParticles < Time.time)
        {
            for (int i = 0; i < laserSpread; i++)
            {
                Vector2 spawnPosition = startDirection * 10 + (i - laserSpread/2) * spreadDireciton * 2;
                if (i % 2 == 0)
                {
                    makeClone(spawnPosition.x, spawnPosition.y, GameManager.IsWhat.Photon_up, -startDirection);
                }
                else
                {
                    makeClone(spawnPosition.x, spawnPosition.y, GameManager.IsWhat.Photon_down, -startDirection);
                }
                
            }
            timerToSpawnParticles = Time.time + 1 / Spawnrate * 2;
        }
    }

    public void RestartFromLastCheckpoint()
    {
        playingState = lastPlayingState;
        faseStarting = true;
        OnPlayingStateChanged?.Invoke(this, EventArgs.Empty);
        diedTimer = 0;
        diedStarting = true;
    }
}
