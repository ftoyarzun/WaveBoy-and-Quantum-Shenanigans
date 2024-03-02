using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public class GameManager : MonoBehaviour
{
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

    public enum PlayingState
    {
        Fase1,
        Fase2,
        Fase3,
        Boss1,
        Fase4,
        Fase5,
        Boss2
    }

    public static GameManager instance { get; private set; }
    public float Timer;
    public TMP_Text Highscore, Combo;
    public int highscore = 0;
    public int combo = 0;
    public int MaxCombo = 0;
    public GameState gameState;
    public static bool gameIsPaused;
    public int difficulty = 0;
    public bool act;
    public float Spawnrate = 2;

    [SerializeField] private ParticleSOList particleSOList;


    public Vector2 randomSpawnPosition;
    public Vector2 aimAtPlayer;


    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetCombo();

    }

    // Update is called once per frame
    void Update()
    {
        if (Timer < Time.time)
        {
            makeClone(-1, -1);
            makeClone(-1, 1);
            makeClone(1, -1);
            makeClone(1, 1);

            Timer = Time.time + 1 / Spawnrate * 2;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    public void ChangeGameState(GameState newstate)
    {

    }

    public void makeClone(int x, int y)
    {
        if (Random.Range(0, 10) > 5)
        {
            randomSpawnPosition = new Vector2(x * Random.Range(8, 12), y * Random.Range(8, 12));
            aimAtPlayer = -(randomSpawnPosition - (Vector2) Player.Instance.transform.position);
            Particle.SpawnParticle(particleSOList.particleSOList[Random.Range(0,2+ difficulty)], randomSpawnPosition, aimAtPlayer, true, 10f);
            //Wavelet clone = Instantiate(wavelet, new Vector3(x * Random.Range(4, 8), y * Random.Range(4, 8)), Quaternion.identity);
            //clone.Starter(true, (float)Random.Range(10, 20) / 10, Random.Range(0, 2+difficulty));
        }
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

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    /*public void SpawnPhotons(Vector2 pos, Vector2 vel)
    {
        Wavelet clone1 = Instantiate(wavelet, pos, Quaternion.identity);
        clone1.Starter(false, 4.0f, IsWhat.Photon_up);
        clone1.SetTimerTo(0.25f);
        clone1.GetComponent<Rigidbody2D>().AddForce(vel * 400);
        Wavelet clone2 = Instantiate(wavelet, pos, Quaternion.identity);
        clone2.Starter(false, 4.0f, IsWhat.Photon_down);
        clone2.SetTimerTo(0.25f);
        clone2.GetComponent<Rigidbody2D>().AddForce(-vel * 400);
    }*/

    public static ParticleSO GetParticleSOFromIsWhat(GameManager.IsWhat isWhat)
    {
        foreach (ParticleSO particleSO in instance.particleSOList.particleSOList)
        {
            if (isWhat == particleSO.isWhat)
            {
                return particleSO;
            }
        }
        return null;
    }

}

public enum GameState
{
    GameStart = 0,
    Pause = 1,
    DieScreen = 2
}
