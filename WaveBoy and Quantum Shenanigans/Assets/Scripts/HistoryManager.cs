using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryManager : MonoBehaviour
{
    private string[] historicalBackground = new string[]
    {
        "The Stern-Gerlach experiment, conducted in 1922 by Otto Stern and Walther Gerlach, is a foundational experiment in quantum mechanics.",
        "It involves passing a beam of particles, typically silver atoms or electrons, through an inhomogeneous magnetic field.",
        "Contrary to classical expectations, the particles do not spread uniformly but instead split into discrete paths, aligning themselves either with the magnetic field's direction or against it.",
        "This outcome confirmed one of the fundamental principles of quantum mechanics: that certain physical properties, like particle spin, are quantized and can only have specific values.",
        "Now, more than a hundred years later, an unsuspecting scientist trying to replicate the experiment is going to have the discovery of his life..."
    };

    [SerializeField] private TextMeshProUGUI fisrtText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI thirdText;
    [SerializeField] private TextMeshProUGUI fourthText;
    [SerializeField] private TextMeshProUGUI fifthText;
    [SerializeField] private TextWriter textWriter;
    [SerializeField] private GameObject textAligner;
    [SerializeField] private ExperimentVideoManager experimentVideoManager;

    //[SerializeField] private 

    private int stringShowingIndex = 0;
    private float timerToStartWriting = 0f;
    private float timerToStartWritingMax = 1f;
    private float timerToWaitBetweenText;
    private float timerToWaitBetweenTextMax = 2f;
    private float characterWrittingRate = 0.02f;

    private bool showVideo = true;

    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();

    private void Awake()
    {
        textList.Add(fisrtText);
        textList.Add(secondText);
        textList.Add(thirdText);
        textList.Add(fourthText);
        textList.Add(fifthText);
        timerToWaitBetweenText = timerToWaitBetweenTextMax;
    }

    private void Update()
    {
        timerToStartWriting += Time.deltaTime;
        if (timerToStartWriting >= timerToStartWritingMax && stringShowingIndex < textList.Count) 
        {
            if (stringShowingIndex == 0)
            {
                textWriter.AddWriter(fisrtText, historicalBackground[0], characterWrittingRate, true);
                stringShowingIndex++;
                return;
            }
            if (textWriter.GetFinishedWritting())
            {
                timerToWaitBetweenText -= Time.deltaTime;
                if (timerToWaitBetweenText < 0)
                {
                    textWriter.AddWriter(textList[stringShowingIndex], historicalBackground[stringShowingIndex], characterWrittingRate, true);
                    stringShowingIndex++;
                    timerToWaitBetweenText = timerToWaitBetweenTextMax;
                    return;
                }
            }
        }

        if (timerToStartWriting > 35f && showVideo)
        {
            //stringShowingIndex = textList.Count;
            showVideo = false;
            HideText();
            experimentVideoManager.Show();
            UIScientist.Instance.Show();
            //experimentVideoManager.PlayVideo();
            UIScientist.Instance.SetHasToDispayText();
        }
    }

    public void ShowText()
    {
        textAligner.SetActive(true);
    }

    public void HideText()
    {
        textAligner.SetActive(false);
    }
}
