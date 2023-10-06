using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstacleGenrator))]
public class LandManager : MonoBehaviour
{
    [SerializeField]
    public GameObject Land;
    public static LandManager Instance { get; private set; }

    public static Tuple<List<LandSaveState>, List<CropSaveState>> farmData = null;

    [SerializeField]
    private List<Land> landPlots = new List<Land>();

    [SerializeField]
    private List<LandSaveState> landData = new List<LandSaveState>();
    [SerializeField]
    private List<CropSaveState> cropData = new List<CropSaveState>();
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

    private void OnEnable()
    {
        RegisterLandPlots();
        StartCoroutine(LoadFarmData());
    }
    IEnumerator LoadFarmData()
    {
        yield return new WaitForEndOfFrame();
        if (farmData != null)
        {
            ImportLandData(farmData.Item1);
            ImportCropData(farmData.Item2);
        }
        else
        {
            GetComponent<ObstacleGenrator>().GenerateObstacles(landPlots);
        }
    }

    private void OnDestroy()
    {
        farmData = new Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
        cropData.ForEach((CropSaveState crop) => {
            
            Debug.Log(crop.seedToGrow);
        });
    }
    #region Registering and Deregistering
    void RegisterLandPlots()
    {
        foreach(Transform landTransform in transform)
        {
            Land land = landTransform.GetComponent<Land>();
            landPlots.Add(land);

            landData.Add(new LandSaveState());

            land.id = landPlots.Count - 1;
 
        }
    }

    public void RegisterCrop(int landID, SeedData seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        cropData.Add(new CropSaveState(landID, seedToGrow.name, cropState, growth, health));
    }

    public void DeregisterCrop(int landID)
    {
        cropData.RemoveAll(x => x.landID == landID);

    }
    #endregion
    public void OnLandStatusChange(int id, Land.LandStatus landStatus, GameTimeStamp lastWatered, Land.FarmObstacleStatus obstacleStatus)
    {
        landData[id] = new LandSaveState(landStatus, lastWatered, obstacleStatus);    
    }

    #region State Change
    public void OnCropStateChange(int landID, CropBehaviour.CropState cropState, int growth, int health)
    {
        int cropIndex = cropData.FindIndex(x => x.landID == landID);
        if (cropIndex >= 0)
        {
            string seedToGrow = cropData[cropIndex].seedToGrow;

            cropData[cropIndex] = new CropSaveState(landID, seedToGrow, cropState, growth, health);
        }
    }
    #endregion

    #region Loading Data
    public void ImportLandData(List<LandSaveState> landDatasetToLoad)
    {
        for (int i = 0; i < landDatasetToLoad.Count; i++)
        {
            LandSaveState landDataToLoad = landDatasetToLoad[i];
            landPlots[i].LoadLandData(landDataToLoad.landStatus, landDataToLoad.lastWatered, landDataToLoad.obstacleStatus);
        }

        landData = landDatasetToLoad;
    }

    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        cropData = cropDatasetToLoad;
        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            Land landToPlant = landPlots[cropSave.landID];
            CropBehaviour cropToPlant = landToPlant.SpawnCrop();
            SeedData seedToGrow = (SeedData)InventoryManager.Instance.itemIndex.GetItemFromString(cropSave.seedToGrow);
            cropToPlant.LoadCrop(cropSave.landID,seedToGrow, cropSave.cropState, cropSave.growth, cropSave.health);
        }
        
    }
    #endregion
}
