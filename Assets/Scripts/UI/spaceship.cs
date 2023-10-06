using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceship : MonoBehaviour
{
    
    private void OnMouseDown()
    {
        bool isMapActive = UIManager.Instance.IsMapPanelActive();
        if (UIManager.Instance.InventoryPanel.activeSelf)
        {
            return;
        }

        if (!isMapActive)
        {
            UIManager.Instance.ToggleImapPanel();
        }
       
    }
}
