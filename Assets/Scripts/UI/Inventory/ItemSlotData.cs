using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;

    public ItemSlotData(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }

    public ItemSlotData(ItemData itemData)
    {
        this.itemData = itemData;
        quantity = 1;
        ValidateQuantity();
    }

    public ItemSlotData (ItemSlotData slotToClone)
    {
        itemData = slotToClone.itemData;
        quantity = slotToClone.quantity;
    }

    public void AddQuantity()
    {
        AddQuantity(1);
    }
    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd;
    }

    public void Remove()
    {
        quantity--;
        ValidateQuantity();
    }

    public bool Stackable(ItemSlotData SlotToComapare)
    {
        return SlotToComapare.itemData == itemData;
    }

    private void ValidateQuantity()
    {
        if(quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }

    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }

    public static ItemSlotDataSave serializeData(ItemSlotData itemslot)
    {
        return new ItemSlotDataSave(itemslot);
    }

    public static ItemSlotData DeserializeData(ItemSlotDataSave itemSaveSlot)
    {
        ItemData item = InventoryManager.Instance.itemIndex.GetItemFromString(itemSaveSlot.itemID);
        return new ItemSlotData(item, itemSaveSlot.quantity);
    }

    public static ItemSlotDataSave[] serializeArray(ItemSlotData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotData, ItemSlotDataSave>(serializeData));
    }

    public static ItemSlotData[] DeserializeArray(ItemSlotDataSave[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotDataSave, ItemSlotData>(DeserializeData));
    }
}


