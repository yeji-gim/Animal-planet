using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    [TextArea(2,5)]
    public string message;

    public string first_button;
    public string second_button;

    public DialogueLine(string speaker, string message)
    {
        this.speaker = speaker;
        this.message = message;
    }

    public DialogueLine(string speaker, string message, string first_button, string second_button)
    {
        this.speaker = speaker;
        this.message = message;
        this.first_button = first_button;
        this.second_button = second_button;
    }


}
