using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingSpeedManager : MonoBehaviour
{
    // Use Rich Text
    // https://docs.unity3d.com/550/Documentation/Manual/StyledText.html
    
    [SerializeField]
    private TMP_Text typingText;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TMP_Text speedText;

    [SerializeField]
    private TMP_Text accuracyText;

    private float startTime = 0f;

    private enum CharState {
        Unknown,
        Correct,
        Wrong
    }

    private string greenColor = "<color=#008000ff>";
    private string redColor = "<color=#ff0000ff>";
    private string stopColor = "</color>";

    private readonly string textToType = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

    private void Awake()
    {
        typingText.text = textToType;
    }

    private void SetText(string text)
    {
        typingText.text = text;
    }

    public void OnTextChange()
    {
        if (startTime == 0f)
            startTime = Time.time;
        string text = GetTextToShow(inputField.text);
        SetText(text);
    }

    string GetTextToShow(string text)
    {
        string finalText = "";
        int correctCount = 0;
        CharState state = CharState.Unknown;
        for(int i = 0; i < textToType.Length; i++)
        {
            if(i < text.Length)
            {
                if(textToType[i] == text[i])
                {
                    correctCount++;
                    if(state == CharState.Wrong)
                        finalText += stopColor;
                    if(state != CharState.Correct)
                        finalText += greenColor;
                    state = CharState.Correct;
                }
                else
                {
                    if (state == CharState.Correct)
                        finalText += stopColor;
                    if (state != CharState.Wrong)
                        finalText += redColor;
                    state = CharState.Wrong;
                }

            }
            else
            {
                if(state != CharState.Unknown)
                    finalText += stopColor;
                state = CharState.Unknown;

            }
            finalText += textToType[i];
        }
        
        if(state != CharState.Unknown)
        {
            finalText += stopColor;
        }
        SetAccuracy(correctCount, text.Length);
        return finalText;
    }

    void SetAccuracy(int correct, int total)
    {
        float accuracy = (total > 0) ? ((float)correct / (float)total) * 100f : 0f;
        accuracyText.text = "Accuracy: \n" + (int)accuracy + "%";
    }

    void SetSpeed()
    {
        if(startTime != 0f)
        {
            float speed = inputField.text.Length / (Time.time - startTime) * 60f;
            speedText.text = "Speed: \n" + (int)speed + "CPM";
        }
    }

    private void Update()
    {
        SetSpeed();
    }
}
