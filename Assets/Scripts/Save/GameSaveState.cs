using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;

    public ItemSlotDataSave[] toolSlots;
    public ItemSlotDataSave[] itemSlots;

    public ItemSlotDataSave equippedItemSlot;
    public ItemSlotDataSave eqiippedToolSlot;
    public GameTimeStamp timeStamp;

    public int money;

    public List<EggIncubatorSaveState> eggIncubating;

    public GameSaveState(
        List<LandSaveState> landData,
        List<CropSaveState> cropData,
        ItemSlotData[] toolSlots,
        ItemSlotData[] itemSlots,
        ItemSlotData equippedItemSlot,
        ItemSlotData equippedToolSlot,
        GameTimeStamp timeStamp,
        int money,
        List<EggIncubatorSaveState> eggIncubating
        )
    {
        this.landData = landData;
        this.cropData = cropData;
        this.toolSlots = ItemSlotData.serializeArray(toolSlots);
        this.itemSlots = ItemSlotData.serializeArray(itemSlots);
        this.equippedItemSlot = ItemSlotData.serializeData(equippedItemSlot);
        this.eqiippedToolSlot = ItemSlotData.serializeData(equippedToolSlot);
        this.timeStamp = timeStamp;
        this.money = money;
        this.eggIncubating = eggIncubating;
    }
}
