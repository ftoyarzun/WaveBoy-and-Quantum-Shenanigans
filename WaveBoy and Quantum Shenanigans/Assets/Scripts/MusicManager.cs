using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager instance { get; private set; }

    [SerializeField] MusicRafsSO musicRafsSO;

    private AudioSource audioSource;
    private float volume = 0.1f;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.5f);
        audioSource.volume = volume;
    }

    private void Start()
    {
        GameManager.Instance.OnPlayingStateChanged += GameManager_OnPlayingStateChanged;
    }

    private void GameManager_OnPlayingStateChanged(object sender, System.EventArgs e)
    {
        switch (GameManager.Instance.GetPlayingState())
        {
            case GameManager.PlayingState.Fase1:
                audioSource.clip = musicRafsSO.music[0];
                break;
            case GameManager.PlayingState.Fase2:
                break;
            case GameManager.PlayingState.Fase3:
                audioSource.clip = musicRafsSO.music[2];
                audioSource.Play();
                break;
            case GameManager.PlayingState.Fase4:
                audioSource.clip = musicRafsSO.music[3];
                audioSource.Play();
                break;
            case GameManager.PlayingState.Fase5:
                audioSource.clip = musicRafsSO.music[4];
                audioSource.Play();
                break;
            case GameManager.PlayingState.Fase6:
                audioSource.clip = musicRafsSO.music[5];
                audioSource.Play();
                break;
            case GameManager.PlayingState.Boss1:
                audioSource.clip = musicRafsSO.boss[0];
                audioSource.Play();
                break;
            case GameManager.PlayingState.Boss2:
                break;
            case GameManager.PlayingState.Died:
                break;
        }
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        volume = (volume > 1f) ? 0f : volume;
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
