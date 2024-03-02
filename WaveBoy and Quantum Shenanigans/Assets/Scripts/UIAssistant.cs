using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAssistant : MonoBehaviour
{

    public event EventHandler OnClickInText;

    [SerializeField] private TextMeshProUGUI assistantText;
    [SerializeField] private TextWriter textWriter;
    [SerializeField] private Button assistantBtn;
    [SerializeField] private GameObject scientistVisuals;

    private int introductionMessagesIndex = 0;


    private string[] introductionMessages = new string[]
    {
        "Scientist: IT IS NOT POSSIBLE! This should not have happened...",
        "Every particle should have gone to the upper or lower stream...",
        "What is happening!! It looks like it is moving using WASD or a gamepad left stick...",
        "This is impossible, it is an abnormality...",
        "it is...",
        "an ABERRATION"
    };

    private void Start()
    {
        textWriter.AddWriter(assistantText, introductionMessages[0], 0.05f, true);
        assistantBtn.onClick.AddListener(() =>
        {
            if (textWriter.GetFinishedWritting())
            {
                textWriter.AddWriter(assistantText, NextStringToWrite(), 0.05f, true);
            }
            else
            {
                OnClickInText?.Invoke(this, EventArgs.Empty);
            }
        });
    }

    private string NextStringToWrite()
    {
        introductionMessagesIndex++;
        return introductionMessages[introductionMessagesIndex];
    }
}
