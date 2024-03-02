using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [SerializeField] UIAssistant uiAssistant;
    private TextMeshProUGUI uiText;
    private string textToWrite;
    private float timePerCharacter;
    private float timer;
    private bool invisibleCharacters;
    private bool finishedWritting = true;
    int characterIndex;

    public void AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        this.invisibleCharacters = invisibleCharacters;
        characterIndex = 0;
    }

    private void Start()
    {
        uiAssistant.OnClickInText += UiAssistant_OnClickInText;
    }

    private void UiAssistant_OnClickInText(object sender, System.EventArgs e)
    {
        if (!finishedWritting)
        {
            finishedWritting = true;
            uiText.text = textToWrite;
            uiText = null;
        }
    }

    private void Update()
    {
        if (uiText != null)
        {
            finishedWritting = false;
            timer -= Time.deltaTime;
            while (timer < 0)
            {
                // Display next character
                timer += timePerCharacter;
                characterIndex++;
                string textForInvisibleCharacters = textToWrite.Substring(0, characterIndex);
                if (invisibleCharacters)
                {
                    textForInvisibleCharacters += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                }
                uiText.text = textForInvisibleCharacters;

                if (characterIndex >= textToWrite.Length)
                {
                    uiText = null;
                    finishedWritting = true;
                    return;
                }
            }
        }
    }

    public bool GetFinishedWritting()
    {
        return finishedWritting;
    }
}
