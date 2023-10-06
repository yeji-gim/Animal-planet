using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandelInventory : InventorySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.HandToInventory(inventoryType);
    }
}
