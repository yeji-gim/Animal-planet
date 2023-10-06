using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public static InventoryManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }

    }
    public InventorySlot inventorySlot;
    public ItemIndex itemIndex;
    [Header("Tools")]
    [SerializeField]private ItemSlotData[] toolSlots = new ItemSlotData[8];
    [SerializeField]private ItemSlotData equippedTool = null;
    [Header("Items")]
    [SerializeField]private ItemSlotData[] itemSlots = new ItemSlotData[8];
    [SerializeField]private ItemSlotData equippedItem = null;
    
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject HandPoint;
    private Transform handPoint;
    private void Start()
    {
        handPoint = HandPoint.transform;

    }
    
    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedTool, ItemSlotData[] itemSlots, ItemSlotData equippedItem)
    {
        this.toolSlots = toolSlots;
        this.equippedItem = equippedItem;
        this.itemSlots = itemSlots;
        this.equippedTool = equippedTool;

        UIManager.Instance.RenderInventory();
        RenderHand();
    }

    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handToequip = equippedTool;
        ItemSlotData[] inventoryToAlter = toolSlots;

        if(inventoryType == InventorySlot.InventoryType.Item)
        {
            handToequip = equippedItem;
            inventoryToAlter = itemSlots;
        }

        if(handToequip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            handToequip.AddQuantity(slotToAlter.quantity);

            slotToAlter.Empty();
        }
        else
        {

            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            inventoryToAlter[slotIndex] = new ItemSlotData(handToequip);

            if(slotToEquip.IsEmpty())
            { 
                handToequip.Empty();
            }
            else
            {
                EquipHandSlot(slotToEquip);
            }
        }
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }
        UIManager.Instance.RenderInventory();
       
    }
    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = equippedTool;

        ItemSlotData[] inventoryToAlter = toolSlots;
        if(inventoryType == InventorySlot.InventoryType.Item)
        {
            handSlot = equippedItem;
            inventoryToAlter = itemSlots;
        }

        if(!StackItemToInventory(handSlot, inventoryToAlter))
        {

            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {

                    inventoryToAlter[i] = new ItemSlotData(handSlot);

                    handSlot.Empty();
                    break;
                }
            }
        }
        
        if(inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }

        UIManager.Instance.RenderInventory();
    }

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for(int i=0; i<inventoryArray.Length; i++)
        {
            if(inventoryArray[i].Stackable(itemSlot))
            {

                inventoryArray[i].AddQuantity(itemSlot.quantity);
  
                itemSlot.Empty();
                return true;
            }
        }
        return false;
    }


    public void ShopToInventory(ItemSlotData itemSlotToMove)
    {
        ItemSlotData[] inventoryToAlter = IsTool(itemSlotToMove.itemData) ? toolSlots : itemSlots;

        if (!StackItemToInventory(itemSlotToMove, inventoryToAlter))
        {

            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(itemSlotToMove);
                    break;
                }
            }
        }

        UIManager.Instance.RenderInventory();
    }

    public void RenderHand()
    {
        if (handPoint != null)
        {
            if (handPoint.childCount > 0)
            {
                if (handPoint.GetChild(0).gameObject != null)
                {
                    Destroy(handPoint.GetChild(0).gameObject);
                }
            }
        }
        if (SlotEquipped(InventorySlot.InventoryType.Item))
        {
            Instantiate(GetEquippedSlotItem(InventorySlot.InventoryType.Item).gameModel, handPoint);
        }
    }



    #region Get functions

    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItem.itemData;
        }
        return equippedTool.itemData;
    }

    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItem;
        }
        return equippedTool;
    }

    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == InventorySlot.InventoryType.Item)
        {
            return itemSlots;
        }

        return toolSlots;
    }


    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == InventorySlot.InventoryType.Item)
        {
            return !equippedItem.IsEmpty();
        }
        return !equippedTool.IsEmpty();
    }


    public bool IsTool(ItemData item)
    {
        EquipmentData equipment = item as EquipmentData;
        if(equipment != null)
        {
            return true;
        }

        SeedData seed = item as SeedData;
        return seed != null;


    }
    #endregion
    public void EquipHandSlot(ItemData item)
    {
       if(IsTool(item))
        {
            equippedTool = new ItemSlotData(item);
        }
       else
        {
            equippedItem = new ItemSlotData(item);
        }
    }

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        ItemData item = itemSlot.itemData;
        if (IsTool(item))
        {
            equippedTool = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItem = new ItemSlotData(itemSlot);
        }
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if(itemSlot.IsEmpty())
        {
            Debug.Log("There is nothing to consume!");
            return;
        }
        itemSlot.Remove();
        RenderHand();
        UIManager.Instance.RenderInventory();
    }
    #region Inventory Slot validation
    private void OnValidate()
    {
        ValidateInventorySlots(equippedTool);
        ValidateInventorySlots(equippedItem);

 
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }

    void ValidateInventorySlots(ItemSlotData slot)
    {
        if(slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }

    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach(ItemSlotData slot in array)
        {
            ValidateInventorySlots(slot);
        }
    }
    #endregion
   
}
