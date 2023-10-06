using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class namingPrompt : MonoBehaviour
{
    [SerializeField]
    TMP_Text promptText;
    [SerializeField]
    TMP_InputField InputField;
    Action<string> onConfirm;
    Action onPromptComplete;
    public PlayerController playerController;
    public void CreatePrompt(string message, Action<string> onConfirm)
    {
        this.onConfirm = onConfirm;
        promptText.text = message;
    }


    public void QueuePormptAction(Action action)
    {
        onPromptComplete += action;
    }


    public void Confirm()
    {
        playerController.SetCanMove(false);
        onConfirm?.Invoke(InputField.text);

        onConfirm = null;
        InputField.text = "";
        gameObject.SetActive(false);

        onPromptComplete?.Invoke();
        onPromptComplete = null;
        playerController.SetCanMove(true);
    }
}
