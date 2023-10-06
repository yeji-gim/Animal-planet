using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AnimalStats : MonoBehaviour
{
    public static List<AnimalRelationShip> animalRelationShips = new List<AnimalRelationShip>();

    static List<AnimalData> animals = Resources.LoadAll<AnimalData>("UI/Data/Animals").ToList();

    public static void StartAnimalCreation(AnimalData animalType)
    {
        if (UIManager.Instance != null)
        {
            animalRelationShips.Add(new AnimalRelationShip(animalType));
        }

    }
    public static void LoadStats(List<AnimalRelationShip> relationshipsToLoad)
    {
        if (relationshipsToLoad == null)
        {
            animalRelationShips = new List<AnimalRelationShip>();
            return;
        }
        animalRelationShips = relationshipsToLoad;
    }

    public static AnimalData GetAnimalTypeFromString(string name)
    {
        return animals.Find(i => i.name == name);
    }
}
