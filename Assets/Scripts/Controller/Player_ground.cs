using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ground : MonoBehaviour
{
    private Vector3 groundOffset;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player"); 

        groundOffset = player.transform.position - FindGroundPosition(player.transform.position);
    }

    private void Start()
    {
        FixPlayerPosition();
    }

    private void FixPlayerPosition()
    {

        GameObject player = GameObject.Find("Player"); 

        Vector3 groundPosition = FindGroundPosition(player.transform.position);

        player.transform.position = groundPosition + groundOffset;
    }

    private Vector3 FindGroundPosition(Vector3 startPosition)
    {
        Debug.Log("Find ground");
        RaycastHit hit;
        if (Physics.Raycast(startPosition, Vector3.down, out hit))
        {
            return hit.point;
        }

        return startPosition;
    }
}
