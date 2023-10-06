using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseCameraController : MonoBehaviour
{
    public Transform player;     // 플레이어의 Transform 컴포넌트
    public float height = 3.0f;     // 카메라의 높이
    public Vector3 offset;
    private Vector3 cameraOffset;   // 카메라의 위치


    void Start()
    {
        // 카메라의 초기 위치 설정
        cameraOffset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // 카메라의 위치 계산
        float currentAngle = player.eulerAngles.y;
        transform.position = player.position + cameraOffset;

        // 카메라의 시선 방향 계산
        transform.LookAt(player.position );
    }
}
