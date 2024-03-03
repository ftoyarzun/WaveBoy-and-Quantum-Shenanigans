using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ExperimentVideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject screenRender;


    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        Loader.LoaderCallback();
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
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
