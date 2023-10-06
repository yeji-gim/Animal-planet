using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShippingShop : MonoBehaviour
{
    [Header("dialogueLine")]
    public List<DialogueLine> forshop;
    public Button first, second;
    [Header("shopItem")]
    public List<ItemData> shopItem;

    public GameObject spaceman;

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;

        if (PlayerStats.Money >= totalCost)
        {
            PlayerStats.Spend(totalCost);
            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);

            InventoryManager.Instance.ShopToInventory(purchasedItem);
        }
    }
    private void OnMouseDown()
    {
        if (UIManager.Instance.InventoryPanel.activeSelf)
        {
            Debug.Log("OnMouseDown");
            return;
        }
        
        else
        {
            Debug.Log("NPC Clicked!");
            if (DialogueManager.Instance != null)
            {
             
                DialogueManager.Instance.StartDialogue(forshop);
            }
        }
 

    }
}
