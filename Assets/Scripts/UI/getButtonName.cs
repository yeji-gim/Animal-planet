using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class getButtonName : MonoBehaviour, IPointerClickHandler
{
    public static string button_name { get; private set; }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedButton = eventData.pointerCurrentRaycast.gameObject;
        button_name = clickedButton.name;
       Debug.Log("Clicked button name: " + button_name);
    }
}
