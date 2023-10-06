using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotDataSave
{
    public string itemID;
    public int quantity;

    public ItemSlotDataSave(ItemSlotData data)
    {
        if(data.IsEmpty())
        {
            itemID = null;
            quantity = 0;
            return;
        }

        itemID = data.itemData.name;
        quantity = data.quantity;

    }
}
