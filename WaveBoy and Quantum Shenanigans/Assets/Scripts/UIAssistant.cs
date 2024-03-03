using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAssistant : MonoBehaviour
{
    public static UIAssistant Instance { get; private set; }

    public event EventHandler OnFinishedDisplayingText;

    [SerializeField] private TextMeshProUGUI assistantText;
    [SerializeField] private TextWriter textWriter;
    [SerializeField] private Button assistantBtn;
    [SerializeField] private GameObject scientistVisuals;

    private int stringShowingIndex = 0;

    private float timerToStartWriting = 0f;
    private float timerToStartWritingMax = 1f;
    private float timerToWaitBetweenText;
    private float timerToWaitBetweenTextMax = 2f;
    private float characterWrittingRate = 0.02f;

    private bool hasToDisplayText = true;


    private string[] introductionMessages = new string[]
    {
        "Scientist: IT IS NOT POSSIBLE! This should not have happened...",
        "Every particle should have gone to the upper or lower stream...",
        "What is happening!! It looks like it is moving using WASD or a gamepad left stick...",
        "This is impossible, it is an abnormality...",
        "it is...",
        "an ABERRATION"
    };

    private string[] PhotonTutorial = new string[]
    {
        "Scientist: Maybe it glows by shooting photons with the left or right mouse click...",
        " or with the triggers on a gamepad...",
        "but might be better for it not be hit by one, it can be very fragile...",
        "Photons are interesting particles, they are their own anti-particle...",
        "collide two of the same and they will annihilate eachother."
    };

    private string[] fase3String = new string[]
    {
        "Scientist: Let us shine some light on it to see what happens ",
    };

    private string[] fase4String = new string[]
    {
        "Scientist: Hmmm it looks like it is eating the photons...",
        "All the light I send in gets absorved somehow...",
        "I really wish I could look what is going on in there...",
        "Maybe if we try a Laser it will react differently...",
    };



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (hasToDisplayText)
        {
            switch (GameManager.instance.GetPlayingState())
            {
                case GameManager.PlayingState.Fase1:
                    DisplayText(introductionMessages);
                    break;
                case GameManager.PlayingState.Fase2:
                    DisplayText(PhotonTutorial);
                    break;
                case GameManager.PlayingState.Fase3:
                    DisplayText(fase3String);
                    break;
                case GameManager.PlayingState.Fase4:
                    DisplayText(fase4String);
                    break;
                case GameManager.PlayingState.Fase5:
                    break;
                case GameManager.PlayingState.Boss1:
                    break;
                case GameManager.PlayingState.Boss2:
                    break;
            }
        }
    }

    private void DisplayText(string[] stringList)
    {
        if (hasToDisplayText)
        {
            timerToStartWriting += Time.deltaTime;
            if (timerToStartWriting >= timerToStartWritingMax && stringShowingIndex < stringList.Length)
            {
                if (textWriter.GetFinishedWritting())
                {
                    timerToWaitBetweenText -= Time.deltaTime;
                    if (timerToWaitBetweenText < 0)
                    {
                        textWriter.AddWriter(assistantText, stringList[stringShowingIndex], characterWrittingRate, true);
                        stringShowingIndex++;
                        timerToWaitBetweenText = timerToWaitBetweenTextMax;
                        return;
                    }
                }
            }
            if (stringShowingIndex == stringList.Length)
            {
                hasToDisplayText = false;
                OnFinishedDisplayingText?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void SetHasToDispayText()
    {
        hasToDisplayText = true;
        //timerToStartWriting = 0f;
        timerToWaitBetweenText = 0f;
        stringShowingIndex = 0;
    }

    public bool GetHasToDispayText()
    {
        return hasToDisplayText;
    }
}
