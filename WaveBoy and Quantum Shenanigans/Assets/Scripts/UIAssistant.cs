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

    public const string TRIGGER_STRING = "          ";

    [SerializeField] private TextMeshProUGUI assistantText;
    [SerializeField] private TextWriter textWriter;
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
        "I can't see what is is, it must be a million times smaller than the thickness of a human hair",
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
        "collide two of the same and they will annihilate eachother...",
        TRIGGER_STRING,
        "But it must be careful, energy must allways be conserved...",
        "If it emits somekind of radiation, it has to lose energy"
    };

    private string[] fase3String = new string[]
    {
        "Scientist: Let us shine some light on it to see what happens ",
    };

    private string[] fase4String = new string[]
    {
        "Scientist: Hmmm it looks like it is eating the photons...",
        "All the light I send in gets absorbed somehow...",
        "I really wish I could look what is going on in there...",
        "Maybe if we try a Laser it will react differently",
    };

    private string[] fase5String = new string[]
    {
        "Scientis: Still nothing, I am begining to desesperate...",
        "Maybe I could try with bigger guns: The ELECTRON MICROSCOPE...",
        "But be careful! These might be atracted to it...",
        "and this time the antiparticle is the opposite particle...",
        "I hope that the ABERRATION can shoot its own electron or positrons...",
        "if it could, probably it would be using left Ctrl and the Spacebar...",
        "or with the left/right buttons on a Gamepad...",
        "if it gets hit, maybe it could gain some energy by annihilating some photons...",
        "these photons get created as the energy of the annihilated particles must be conserved"
    };

    private string[] fase6String = new string[]
    {
        "Scientist: I can't understand what is happening...",
        "Do I have to send both electrons and positrons maybe?"
    };

    private string[] Boss1String = new string[]
    {
        "Scientist: Hmmm that did not work, every particle I send in gets eaten",
        "it is like this thing does not want to be seen",
        "I think I have to use the last resort: Take the magnets out such that the beams go to the middle again",
        "This way perhaps one silver atom is going to deal with this aberration",
        "It has a lot more mass than an electron, therefore it should last longer in there"
    };


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        textWriter.OnTriggerString += TextWriter_OnTriggerString;
    }

    private void TextWriter_OnTriggerString(object sender, EventArgs e)
    {
        switch (GameManager.Instance.GetPlayingState())
        {
            case GameManager.PlayingState.Fase1:
                break;
            case GameManager.PlayingState.Fase2:
                PlayerHitPointManager.Instance.Show();
                Player.Instance.ResetPlayerHitPoints();
                break;
            case GameManager.PlayingState.Fase3:
                break;
            case GameManager.PlayingState.Fase4:
                break;
            case GameManager.PlayingState.Fase5:
                break;
            case GameManager.PlayingState.Fase6:
                break;
            case GameManager.PlayingState.Boss1:
                break;
            case GameManager.PlayingState.Boss2:
                break;
        }
    }

    private void Update()
    {
        if (hasToDisplayText)
        {
            switch (GameManager.Instance.GetPlayingState())
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
                    DisplayText(fase5String);
                    break;
                case GameManager.PlayingState.Fase6:
                    DisplayText(fase6String);
                    break;
                case GameManager.PlayingState.Boss1:
                    DisplayText(Boss1String);
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
