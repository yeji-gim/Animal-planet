using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class incubator : InteractableObject
{
    public string name;
    // Whether it has egg placed inside
    bool containEgg;

    // The time it takes to incubate the egg in minutes
    int timeToIncubate;

    // egg to display when the player has played an egg in the incubator
    public GameObject displayEgg;

    public int incubationID;

   
    public void startegg()
    {
        if (CanIncubate())
        {
            StartIncubation();
        }
    }
    private void OnMouseDown()
    {
        startegg();
    }
    // Check if the player is able to throw in an egg to start incubation
    bool CanIncubate()
    {
        // Get the item data the player
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

        // If the player is not holding anything if there is already an egg, he cannot incubate an anything
        if (handSlotItem == null || containEgg) return false;

        // Make sure you set the item to egg
        if (handSlotItem.name != item.name) return false;

        return true;
    }

    // Starts the incubation
    void StartIncubation()
    {
        InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
        int hours = GameTimeStamp.DaysToHour(IncubationManager.daysToIncubate);
        SetIncubationState(true, GameTimeStamp.HoursToMinute(hours));

        // Register it with the list of incubating eggs
        IncubationManager.eggsIncubating.Add(new EggIncubatorSaveState(incubationID, timeToIncubate,name));
    }

    public void SetIncubationState(bool containEgg, int timeToIncubate)
    {
        // Set the values accordingly
        this.containEgg = containEgg;
        this.timeToIncubate = timeToIncubate;

        // Render the egg in the scene if it has an egg
        displayEgg.SetActive(containEgg);
    }
}
