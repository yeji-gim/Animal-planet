using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  
    public float smoothSpeed = 0.125f;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 5f;
    public float maxZoomDistance = 5f;
    public float minZoomDistance = 2f;

    private Vector3 cameraOffset;
    private bool isZoomed = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Transform target;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalFOV;
    void Start()
    {
        cameraOffset = transform.position - player.position;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalFOV = Camera.main.fieldOfView;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + cameraOffset;
            transform.LookAt(player.position + Vector3.up * 3.0f);
        }

    }
    
}
    
   

