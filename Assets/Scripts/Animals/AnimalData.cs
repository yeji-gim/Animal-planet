using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName = "Animals/Animal")]
public class AnimalData : ScriptableObject
{
    public Sprite portrait;
    public AnimalBehaviour animalObject;
    public int purchasePrice;
    public ItemData produce;
    public SceneTransitionManager.Location building;
}
