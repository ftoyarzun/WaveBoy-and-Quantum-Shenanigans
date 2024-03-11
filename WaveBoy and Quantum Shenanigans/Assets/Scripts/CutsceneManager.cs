using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{

    public static CutsceneManager Instance { get; private set; }

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject screenRender;
    [SerializeField] private VideoClip gameOverClip;
    [SerializeField] private VideoClip victoryClip;

    private bool videoFinishedPlaying = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        //Hide();
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        videoFinishedPlaying = true;
    }

    public void PlayVideo()
    {
        videoFinishedPlaying = false;
        switch (GameManager.Instance.GetPlayingState()){
            case GameManager.PlayingState.Died:
                videoPlayer.clip = gameOverClip;
                break;
            case GameManager.PlayingState.Boss2:
                videoPlayer.clip = victoryClip;
                break;
            default:
                break;
        }
        videoPlayer.Play();
    }



    public void Show()
    {
        screenRender.SetActive(true);
        PlayVideo();
    }

    public void Hide()
    {
        screenRender.SetActive(false);
    }

    public bool IsPlaying()
    {
        return !videoFinishedPlaying;
    }
}
