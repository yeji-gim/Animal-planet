using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public ItemSlotData ItemSlotData { get; private set; }
    ItemData displaytoItem;
    int quantity;
    public Image displayItem;
    public TMP_Text quantityText;
    public GameObject check;
    private bool isChecked;
    static int total_price;
    public enum InventoryType
    {
        Item, Tool
    }

    public InventoryType inventoryType;

    void Start()
    {
        Check(false);
        
    }

    public int slotIndex;
    public int gettotoal()
    {
        return total_price;
    }
    public void Display(ItemSlotData itemSlot)
    {
        ItemSlotData = itemSlot;

        displaytoItem = itemSlot.itemData;
        quantity = itemSlot.quantity;

        quantityText.text = "";

        if(displaytoItem != null)
        {
            displayItem.sprite = displaytoItem.thumbnail;

            if(quantity > 1)
            {
                quantityText.text = quantity.ToString();
            }
            displayItem.gameObject.SetActive(true);

            return;
        }

        displayItem.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (DialogueManager.Instance.IsDialoguePanelActive())
        {
            InventorySlot targetSlot = GetComponent<InventorySlot>();
            if (targetSlot.IsChecked())
            {
                targetSlot.Check(false); 
                total_price -= targetSlot.GetCostAsInt() * targetSlot.quantity;

            }
            else
            {
                targetSlot.Check(true); 
                total_price = 0;
                foreach (InventorySlot slot in UIManager.Instance.ItemSlots)
                {
                    if (slot.IsChecked()) 
                    {
                        total_price += slot.GetCostAsInt() * slot.quantity;
                    }
                }
            }
            UIManager.Instance.shopmoney.SetActive(true);
            UIManager.Instance.selectbutton.SetActive(true);
            UIManager.Instance.shopmoneyText.text = total_price.ToString() + "G";
        }
        else
        {
            InventoryManager.Instance.InventoryToHand(slotIndex, inventoryType);
        }   
    }
    public int GetCostAsInt()
    {
        return displaytoItem.cost;
    }
    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    public void Check(bool toggle)
    {
        isChecked = toggle;
        check.SetActive(toggle);

    }
    public bool IsChecked()
    {
        return isChecked;
    }
    public void forSelectedbutton()
    {
        DialogueManager.Instance.UpdateDialogue();
        PlayerStats.Earn(total_price);

        total_price = 0;
        foreach (InventorySlot slot in UIManager.Instance.ItemSlots)
        {
            if (slot.IsChecked()) 
            {
                slot.Check(false);
                slot.ItemSlotData.Empty();
            }
        }
        UIManager.Instance.RenderInventory();
        UIManager.Instance.ToggleInventoryPanel();
        UIManager.Instance.shopmoney.SetActive(false);
        UIManager.Instance.selectbutton.SetActive(false);

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(displaytoItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }
}
