using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ExperimentVideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject screenRender;

    private float generalTimer = 0;
    private float pauseTimer = 0;
    private float pauseTimerMax = Mathf.Infinity;
    private bool videoPaused = false;


    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void Update()
    {
        if (videoPaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer > pauseTimerMax)
            {
                pauseTimer = 0;
                videoPaused = false;
                PlayVideo();
            }
        }
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        Loader.LoaderCallback();
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    public void PauseVideo(float pauseTime)
    {
        videoPaused = true;
        videoPlayer.Pause();
        pauseTimerMax = pauseTime;
    }

    public void Show()
    {
        screenRender.SetActive(true);
    }

    public void Hide()
    {
        screenRender.SetActive(false);
    }
}
