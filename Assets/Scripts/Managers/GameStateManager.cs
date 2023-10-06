using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

    private void Awake()
    {
        var obj = FindObjectsOfType<GameStateManager>();
        // If there is more than one instance, destory the extra
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        // Add this to TimeManager's 
        TimeManager.Instance.RegisterTracker(this);
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        updateFarmState(timeStamp);
        updateShippingState(timeStamp);
        IncubationManager.UpdateEgg();
    }

    void updateShippingState(GameTimeStamp timeStamp)
    {
        Shippingban.ShipItem();
        
    }

    public void ShopTime(GameTimeStamp timeStamp)
    {
        if (timeStamp.hour >= 7 && timeStamp.hour < 20)
        {
            SceneTransitionManager.Instance.SwitchLocation(SceneTransitionManager.Location.shop);
        }
        else
        {
            UIManager.Instance.openNotice.SetActive(true);
        }
    }    
    void updateFarmState(GameTimeStamp timeStamp)
    {
        if (SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm)
        {

            if (LandManager.farmData == null) return;

            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;


            if (cropData.Count == 0) return;

            for (int i = 0; i < cropData.Count; i++)
            {
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                if (crop.cropState == CropBehaviour.CropState.Wilted) continue;


                land.ClockUpdate(timeStamp);

                if (land.landStatus == Land.LandStatus.Watered)
                {
                    crop.Grow();
                }
                else if (crop.cropState != CropBehaviour.CropState.Seed)
                {
                    crop.Wither();
                }
                cropData[i] = crop;
                landData[crop.landID] = land;
            }

        }
    }
    public void sleep()
    {
        StartCoroutine(TransitionTime());
    }

    IEnumerator TransitionTime()
    {

        GameTimeStamp timestampOfNextDay = TimeManager.Instance.GetGameTimeStamp();

        timestampOfNextDay.hour += 6;
        timestampOfNextDay.minute = 0;

        TimeManager.Instance.SkipTime(timestampOfNextDay);
        SaveManager.Save(ExportSaveState());

        yield return new WaitForSeconds(1);
    }

    public GameSaveState ExportSaveState()
    {

        List<LandSaveState> landData = LandManager.farmData.Item1;
        List<CropSaveState> cropData = LandManager.farmData.Item2;


        ItemSlotData[] toolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);

        ItemSlotData equippedToolSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData equippedItemSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);


        GameTimeStamp timeStamp = TimeManager.Instance.GetGameTimeStamp();
        return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedToolSlot, equippedItemSlot, timeStamp,PlayerStats.Money,IncubationManager.eggsIncubating); 
    }

    public void LoadSave()
    {

        GameSaveState save = SaveManager.Load();

        TimeManager.Instance.LoadTime(save.timeStamp);

        // Inventory
        ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(save.toolSlots);
        ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(save.eqiippedToolSlot);
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(save.itemSlots);
        ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(save.equippedItemSlot);
        InventoryManager.Instance.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

        LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData, save.cropData);

        PlayerStats.LoadStats(save.money);
        IncubationManager.eggsIncubating = save.eggIncubating;
    }
}
