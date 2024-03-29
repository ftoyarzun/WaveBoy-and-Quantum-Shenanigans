using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class ScientistVisuals : MonoBehaviour
{
    [SerializeField] Image scientistImage;
    [SerializeField] Sprite surprisedScientist;
    [SerializeField] Sprite wonderingScientist;
    [SerializeField] Sprite ideaScientist;
    [SerializeField] Sprite regretScientist;

    private bool shrinkScientist = false;

    private void Start()
    {
        GameManager.Instance.OnPlayingStateChanged += GameManager_OnPlayingStateChanged;
    }

    private void Update()
    {
        if (shrinkScientist)
        {
            scientistImage.transform.localScale *= 0.99f;
        }
    }

    private void GameManager_OnPlayingStateChanged(object sender, System.EventArgs e)
    {
        switch (GameManager.Instance.GetPlayingState())
        {
            case GameManager.PlayingState.Fase1:
                scientistImage.sprite = surprisedScientist;
                break;
            case GameManager.PlayingState.Fase2:
                scientistImage.sprite = wonderingScientist;
                break;
            case GameManager.PlayingState.Fase3:
                scientistImage.sprite = ideaScientist;
                break;
            case GameManager.PlayingState.Fase4:
                scientistImage.sprite = wonderingScientist;
                break;
            case GameManager.PlayingState.Fase5:
                scientistImage.sprite = regretScientist;
                break;
            case GameManager.PlayingState.Fase6:
                scientistImage.sprite = wonderingScientist;
                break;
            case GameManager.PlayingState.Boss1:
                break;
            case GameManager.PlayingState.Boss2:
                break;
            case GameManager.PlayingState.Died:
                scientistImage.sprite = regretScientist;
                break;
        }
    }

    public void ShrinkScientist()
    {
        shrinkScientist = true;
    }
}
