using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogePrompt : MonoBehaviour
{
    [Header("Button")]
    public GameObject button;
    public Button firstButton;
    public Button secondButton;
    public TMP_Text first;
    public TMP_Text second;

    Action onfistSelected = null;
    Action onsecondSelected = null;

    private void Awake()
    {
        Debug.Log("Awake");
    

    }
    public void Createbutton(string first_text, string second_text, Action firstAction, Action secondAction)
    {
        onfistSelected = firstAction;
        onsecondSelected = secondAction;

        first.text = first_text;
        second.text = second_text;

        firstButton.onClick.AddListener(() => {
            firstAction?.Invoke();
        });

        secondButton.onClick.AddListener(() => {
            secondAction?.Invoke();
        });
    }
    public void OnFirstButtonClicked()
    {
        onfistSelected?.Invoke();
        button.SetActive(false);
    }

    public void OnSecondButtonClicked()
    {
        onsecondSelected?.Invoke();
        button.SetActive(false);
    }
}
