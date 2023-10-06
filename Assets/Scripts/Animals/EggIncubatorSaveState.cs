using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EggIncubatorSaveState
{
    public int incubatorID;
    public int timeToIncubate;
    public string animalName;
    public EggIncubatorSaveState(int incubatorID, int timeToIncubator, string animalName)
    {
        this.incubatorID = incubatorID;
        this.timeToIncubate = timeToIncubator;
        this.animalName = animalName;
    }

    // Call this when the clock updates
    public void Tick()
    {
        timeToIncubate--;
        Debug.Log($"Incubator {incubatorID} has {timeToIncubate} mins remaining");
    }
}
