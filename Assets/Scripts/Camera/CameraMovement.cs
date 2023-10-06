using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // inspector variables
    [SerializeField, Tooltip("Player transform for camera to follow")]
    private Transform playerTransform;
    [SerializeField, Tooltip("Camera offset from player (x not used)")]
    private Vector3 offsetPosition = new Vector3(0, 5, 5);
    [SerializeField]
    private bool lookAt = true;

    // privates
    private Transform _mainCam = null;

    // Use this for initialization
    private void Start()
    {
        if(playerTransform == null)
        {
            Debug.LogError("CameraMovement is missing playerTransform");
        }
        else
        {
            _mainCam = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (playerTransform == null)
        {
            return;
        }
        transform.position = playerTransform.position + -(playerTransform.forward * offsetPosition.z) + (playerTransform.up * offsetPosition.y);
        if (lookAt)
        {
            _mainCam.LookAt(playerTransform, playerTransform.up);
        }
        else
        {
            _mainCam.LookAt(playerTransform);
        }
    }
}
