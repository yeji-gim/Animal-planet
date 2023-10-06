using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class mapButtons : MonoBehaviour
{
    public planet pinkButton;
    public planet blueButton;
    public planet orangeButton;

    private planet currentPlanetButton;


    private void Start()
    {
        pinkButton.Init(500);
        blueButton.Init(1000);
        orangeButton.Init(1500);

    }
    private planet GetPlanetButtonFromGameObject()
    {
        
        if (getButtonName.button_name == "Pink")
        {
            return pinkButton;
        }
        else if (getButtonName.button_name == "Orange")
        {
            return orangeButton;
        }
        else if (getButtonName.button_name == "Blue")
        {
            return blueButton;
        }
        return null;
    }
    public string GetButtonName()
    {
        return getButtonName.button_name;
    }
    public void purchaseButton()
    {
        currentPlanetButton = GetPlanetButtonFromGameObject();
        Debug.Log(currentPlanetButton);
        if (currentPlanetButton != null)
        {
            if (PlayerStats.Money < currentPlanetButton.cost)
            {
                
                UIManager.Instance.planetcost.text = "no money";
            }
            else
            {
                DialogueManager.Purchase(currentPlanetButton.cost);
                currentPlanetButton.Unlock();
                UIManager.Instance.unlockpanel.SetActive(false);
            }
        }
    }
    
    public void cancelButton()
    {
        if (gameObject.CompareTag("unlock"))
        {
            UIManager.Instance.unlockpanel.SetActive(false);

        }
    }


    
}
