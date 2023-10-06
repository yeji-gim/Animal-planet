using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YesNoPrompt : MonoBehaviour
{
    [SerializeField]
    TMP_Text promptText;
    [SerializeField]
    TMP_Text head;
    Action onYesSelected = null;

    public void CreatePrompt(string message,string head, Action onYesSelected)
    {
        this.onYesSelected = onYesSelected;
        promptText.text = message;
        this.head.text = head;
    }

    public void Answer(bool yes)
    {
        if(yes)
        {
            onYesSelected();
        }

        onYesSelected = null;
        gameObject.SetActive(false);
    }
}
