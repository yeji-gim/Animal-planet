using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Charcter/Charcter")]
public class CharcterData : ScriptableObject
{
    [Header("Dialouge")]
    public List<DialogueLine> defaultDialoge;

}
