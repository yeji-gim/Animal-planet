using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehaviour : InteractableObject
{
    AnimalRelationShip relationShip;
    AnimalMovement movement;
    public void LoadRelationShip(AnimalRelationShip relationShip)
    {
        this.relationShip = relationShip;
    }
    private void Start()
    {
        movement = GetComponent<AnimalMovement>();
    }

}
