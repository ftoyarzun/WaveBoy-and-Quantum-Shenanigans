using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiedScreenUI : MonoBehaviour
{
    public static DiedScreenUI Instance { get; private set; }

    [SerializeField] Button restartGameButton;
    [SerializeField] Button restartLastCheckPointButton;
    [SerializeField] Button mainMenuButton;

    private void Awake()
    {
        Instance = this;

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        restartGameButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        restartLastCheckPointButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartFromLastCheckpoint();
            Hide();
            CutsceneManager.Instance.Hide();
        });

    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
