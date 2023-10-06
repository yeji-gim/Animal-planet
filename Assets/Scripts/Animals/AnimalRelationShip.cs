using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRelationShip : MonoBehaviour
{
    public string animalType;

    public AnimalRelationShip(AnimalData animalType)
    {
        this.animalType = animalType.name;
    }


    public AnimalData AnimalType()
    {
        return AnimalStats.GetAnimalTypeFromString(animalType);
    }
}
