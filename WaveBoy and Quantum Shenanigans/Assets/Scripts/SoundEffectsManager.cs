using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFETS_VOLUME = "SoundEffectsVolume";
    public static SoundEffectsManager Instance { get; private set; }
    [SerializeField] private AudioClipRafsSO audioClipRafsSO;
    private float volume = 5f;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFETS_VOLUME, 1f);
    }

    private void Start()
    {
        Particle.OnParticleCollision += Particle_OnParticleCollision;
        Player.Instance.OnShoot += Player_OnShoot;
    }

    private void Player_OnShoot(object sender, Player.OnShootEventArgs e)
    {
        switch (e.isWhat)
        {
            case GameManager.IsWhat.Photon_up:
                PlaySound(audioClipRafsSO.fire[0], Vector2.zero);
                break;
            case GameManager.IsWhat.Photon_down:
                PlaySound(audioClipRafsSO.fire[1], Vector2.zero);
                break;
            case GameManager.IsWhat.Electron:
                PlaySound(audioClipRafsSO.fire[2], Vector2.zero);
                break;
            case GameManager.IsWhat.Positron:
                PlaySound(audioClipRafsSO.fire[3], Vector2.zero);
                break;
            case GameManager.IsWhat.Player:
                break;
        }
    }

    private void Particle_OnParticleCollision(object sender, Particle.OnParticleCollisionEventAgrs e)
    {
        switch (e.isWhat)
        {
            case GameManager.IsWhat.Photon_up:
                PlaySound(audioClipRafsSO.collide[0], Vector2.zero);
                break;
            case GameManager.IsWhat.Photon_down:
                PlaySound(audioClipRafsSO.collide[0], Vector2.zero);
                break;
            case GameManager.IsWhat.Electron:
                PlaySound(audioClipRafsSO.collide[1], Vector2.zero);
                break;
            case GameManager.IsWhat.Positron:
                PlaySound(audioClipRafsSO.collide[1], Vector2.zero);
                break;
            case GameManager.IsWhat.Player:
                break;
        }
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        volume = (volume > 1f) ? 0f : volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFETS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
