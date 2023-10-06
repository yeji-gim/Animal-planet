using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class planet : MonoBehaviour
{
    public int cost;

    // Icon to be displayed in UI
    public GameObject lockImage;

    public Button unlockButton;
    public Button planetButton;
    public bool isLocked = true;

    public void Init(int cost)
    {
        this.cost = cost;
        unlockButton.onClick.AddListener(SetUnlockButton);
        //planetButton.onClick.AddListener(SetPlanetButtonState);
        planetButton.interactable = false;
        unlockButton.interactable = true;
    }

    private void SetUnlockButton()
    {
        Debug.Log("SetUnlockButton");
        UIManager.Instance.unlockpanel.SetActive(true);
        UIManager.Instance.planetcost.text = cost.ToString();
    }

    private void onMouseDown()
    {
        if (!isLocked)
        {
            if (getButtonName.button_name == "Pink")
            {
                SceneTransitionManager.Instance.SwitchLocation(SceneTransitionManager.Location.Alien);
            }
            if (getButtonName.button_name == "Blue")
            {
                SceneTransitionManager.Instance.SwitchLocation(SceneTransitionManager.Location.blue);
            }
        }
    }

    public void Unlock()
    {
        isLocked = false;
        lockImage.SetActive(false);
        planetButton.interactable = true;
        unlockButton.gameObject.SetActive(false);
    }

}
