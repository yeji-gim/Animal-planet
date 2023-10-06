using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IncubationManager : MonoBehaviour
{
    // The list of eggs currently being incubated
    public static List<EggIncubatorSaveState> eggsIncubating = new List<EggIncubatorSaveState>();

    // How long it takes in days for an egg to hatch
    public const int daysToIncubate = 1;

    public List<incubator> incubators;

    public static UnityEvent OnEggUpdate = new UnityEvent();

    private void OnEnable()
    {
        // Assign each incubator an id
        RegisterIncubators();
        // Load up the incubator information
        LoadIncubatorData();
        // Load the data on every update
        OnEggUpdate.AddListener(LoadIncubatorData);
    }

    private void OnDestroy()
    {
        OnEggUpdate.RemoveListener(LoadIncubatorData);
    }
    public static void UpdateEgg()
    {
        // Eggs must be incubating
        if (eggsIncubating.Count == 0) return;

        foreach(EggIncubatorSaveState egg in eggsIncubating.ToList())
        {
            //Update the egg
            egg.Tick();
            OnEggUpdate?.Invoke();
            

            if (egg.timeToIncubate <= 0)
            {
                eggsIncubating.Remove(egg);

                AnimalData animalData = AnimalStats.GetAnimalTypeFromString(egg.animalName);
                if (animalData != null)
                {
                    AnimalStats.StartAnimalCreation(animalData);
                }
            }
        }
    }

   

    // Assign an ID for each incubator
    void RegisterIncubators()
    {
        for(int i = 0; i < incubators.Count; i++)
        {
            incubators[i].incubationID = i;
        }
    }

    void LoadIncubatorData()
    {
        if (eggsIncubating.Count == 0) return;

        foreach(EggIncubatorSaveState egg in eggsIncubating)
        {
            // Get the incubator to load
            incubator incubatorToLoad = incubators[egg.incubatorID];

            bool isIncubating = true;
            //Check if the eggi s hatching/has hatched
            if(egg.timeToIncubate <= 0)
            {
                isIncubating = false;
            }
            incubatorToLoad.SetIncubationState(isIncubating, egg.timeToIncubate);
        }
    }
}
