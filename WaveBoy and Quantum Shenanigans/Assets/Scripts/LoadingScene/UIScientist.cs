using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class UIScientist : MonoBehaviour
{
    public static UIScientist Instance { get; private set; }

    public event EventHandler OnFinishedDisplayingText;

    public const string TRIGGER_STRING = "          ";

    [SerializeField] private TextMeshProUGUI assistantText;
    [SerializeField] private TextWriter textWriter;
    [SerializeField] private ScientistVisualsLoading scientistVisuals;
    [SerializeField] private ExperimentVideoManager experimentVideoManager;

    private int stringShowingIndex = 0;

    private float timerToStartWriting = 0f;
    private float timerToStartWritingMax = 0f;
    private float timerToWaitBetweenText;
    private float timerToWaitBetweenTextMax = 3f;
    private float characterWrittingRate = 0.02f;

    private bool hasToDisplayText = true;

    private int triggers;




    private string[] contextText = new string[]
    {
        "Scientist: Ok, the experiment is all set up.",
        "First, lets get the particle emmiter glowing",
        TRIGGER_STRING,
        "Look at that beautiful particle beam colliding with the fluorescent sheet glowing green...",
        TRIGGER_STRING,
        "Now lets bring the magnets to deflect the particle beam...",
        "Look at those two streams, just what I was expecting.",
        TRIGGER_STRING,
        "Wait a moment, what is that thing in the middle...",
        //TRIGGER_STRING
    };


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        textWriter.OnTriggerString += TextWriter_OnTriggerString;
        Hide();
    }

    private void TextWriter_OnTriggerString(object sender, EventArgs e)
    {
        scientistVisuals.SetScientistImage(triggers);
        switch (triggers)
        {
            case 0:
                experimentVideoManager.PlayVideo();
                break;
            case 1:
                experimentVideoManager.PauseVideo(3.5f);
                timerToWaitBetweenText = -1;
                break;
            case 2:
                experimentVideoManager.PauseVideo(4);
                break;
            case 3:
                //experimentVideoManager.PauseVideo(3);
                break;
            case 4:
                //experimentVideoManager.PlayVideo();
                break;
            default:
                break;
        }
        triggers++;
    }

    private void Update()
    {
        if (hasToDisplayText)
        {
             DisplayText(contextText);
        }
    }

    private void DisplayText(string[] stringList)
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
