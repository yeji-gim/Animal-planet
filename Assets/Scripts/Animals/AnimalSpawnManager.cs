using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawnManager : MonoBehaviour
{
    [SerializeField] Collider floor;
    void Start()
    {
        RenderAnimal();
    }

    public void RenderAnimal()
    {
        foreach(AnimalRelationShip animalRelation in AnimalStats.animalRelationShips)
        {
            AnimalData animalType = animalRelation.AnimalType();

            if(animalType.building == SceneTransitionManager.Instance.currentLocation)
            {
                // Boundaries the coordinates the animal will spwan in based on the boudns of the floor collider
                Bounds bounds = floor.bounds;
                float offSetX = Random.Range(bounds.extents.x - 2 , bounds.extents.x + 2);
                float offSetZ = Random.Range(bounds.extents.z - 5, bounds.extents.z + 5);

                Vector3 spawnPt = new Vector3(offSetX, floor.transform.position.y, offSetZ);

                float randomYRotation = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);

                // Spawn the animal and get the AnimalBehaviour component
                AnimalBehaviour animal = Instantiate(animalType.animalObject,spawnPt, randomRotation);
                // Load in the relationship data
                animal.LoadRelationShip(animalRelation);
            }
        }
    }

}
   
