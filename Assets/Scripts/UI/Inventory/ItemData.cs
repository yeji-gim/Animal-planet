using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item")]
public class ItemData : ScriptableObject
{
    public string description;

    public Sprite thumbnail;

    public GameObject gameModel;

    public int cost;

    public int GetCostAsInt()
    {
        return cost;
    }
}
