using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shippingban : InteractableObject
{
    public static int hourToShip = 12;
    public static List<ItemSlotData> itemToShips = new List<ItemSlotData>();


    public override void Pickup()
    {
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

        if (handSlotItem == null) return;

        UIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ?", "24H",placeItemInShippingBin);
    }

    
    void placeItemInShippingBin()
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);

        itemToShips.Add(new ItemSlotData(handSlot));

        handSlot.Empty();

        InventoryManager.Instance.RenderHand();

        foreach(ItemSlotData item in itemToShips)
        {
            Debug.Log($"in the shipping bin : {item.itemData.name} x {item.quantity}");
        }
    }

    public static void ShipItem()
    {
        int moneyToReceive = TallyItems(itemToShips);
        PlayerStats.Earn(moneyToReceive);
        itemToShips.Clear();
    }
    public static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;
        foreach(ItemSlotData item in items)
        {
            total += (int)(item.quantity * item.itemData.cost * 0.8);
        }
        return total;
    }
}
