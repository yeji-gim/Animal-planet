using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public int id;
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }

    public LandStatus landStatus;
    new Renderer renderer;
    public Material soilMat, farmLandMat, wateredMat;
    public GameObject select;

    GameTimeStamp timewatered;
    
    [Header("Crops")]
    public GameObject cropPrefab;

    CropBehaviour cropPlanted = null;
    
    public enum FarmObstacleStatus { None, Rock, Grass}
    [Header("Obstacles")]
    public FarmObstacleStatus obstacleStatus;
    public GameObject rockPrefab, grassPrefab;
    GameObject obstacleObject;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(LandStatus.Soil);
        Select(false);

        TimeManager.Instance.RegisterTracker(this);

     
    }

    
    public void LoadLandData(LandStatus LandStatusToSwitch, GameTimeStamp lastWatered, FarmObstacleStatus obstacleStatusToSwitch)
    {
        // Set land status accordingly
        landStatus = LandStatusToSwitch;
        timewatered = lastWatered;

        Material materialToSwitch = soilMat;

        // Decide what material to switch to
        switch (LandStatusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmLandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                break;
        }
        switch (obstacleStatusToSwitch)
        {
            case FarmObstacleStatus.None:
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Grass:
                obstacleObject = Instantiate(grassPrefab, transform);
                break;
        }

        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        obstacleStatus = obstacleStatusToSwitch;
        // Get the renderer to apply the changes
        renderer.material = materialToSwitch;
    }

    // Decide what material to switch to
    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;

        // Decide what material to switch to
        switch(statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmLandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                // Cahce the time it was watered
                timewatered = TimeManager.Instance.GetGameTimeStamp();
                break;
        }


        if (renderer != null)
        {
            // Get the renderer to apply the change
            renderer.material = materialToSwitch;
        }
        LandManager.Instance.OnLandStatusChange(id, landStatus, timewatered,obstacleStatus);
    }

    public void SetObstacleStatus(FarmObstacleStatus stateToSwitch)
    {
        switch (stateToSwitch)
        {
            case FarmObstacleStatus.None:
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Grass:
                obstacleObject = Instantiate(grassPrefab, transform);
                break;
        }
        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        obstacleStatus = stateToSwitch;
        LandManager.Instance.OnLandStatusChange(id, landStatus, timewatered, obstacleStatus);
    }
    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    public void Interact()
    {

        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);
        if (!InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Tool))
        {
            return;
        }
        EquipmentData equipmentTool = toolSlot as EquipmentData;


        if(equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.tooltype;

            switch(toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    if(landStatus != LandStatus.Soil)
                    {
                        SwitchLandStatus(LandStatus.Watered);
                    }
                    break;
                case EquipmentData.ToolType.Shovel:
                    // Remove the crop from the land
                    if(cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }

                    break;
                case EquipmentData.ToolType.Axe:
                    if (obstacleStatus == FarmObstacleStatus.Grass) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
                case EquipmentData.ToolType.Pickaxe:
                    if (obstacleStatus == FarmObstacleStatus.Rock) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
            }
            return;
        }
        SeedData seedTool = toolSlot as SeedData;

        if(seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null)
        {
            SpawnCrop();

            cropPlanted.Plant(id,seedTool);

            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        }
    }

    public CropBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropPrefab, transform);
        cropPlanted = cropObject.GetComponent<CropBehaviour>();
        return cropPlanted;
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        
        if (landStatus == LandStatus.Watered)
        {

            int hourElapsed = GameTimeStamp.CompareTimestamps(timewatered, timestamp);

            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }
            if (hourElapsed > 6)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }


        if (landStatus != LandStatus.Watered && cropPlanted != null && obstacleStatus == FarmObstacleStatus.None)
        {
            if (cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }


}
