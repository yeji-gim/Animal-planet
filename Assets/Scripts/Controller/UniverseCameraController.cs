using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseCameraController : MonoBehaviour
{
    public Transform player;     // �÷��̾��� Transform ������Ʈ
    public float height = 3.0f;     // ī�޶��� ����
    public Vector3 offset;
    private Vector3 cameraOffset;   // ī�޶��� ��ġ


    void Start()
    {
        // ī�޶��� �ʱ� ��ġ ����
        cameraOffset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // ī�޶��� ��ġ ���
        float currentAngle = player.eulerAngles.y;
        transform.position = player.position + cameraOffset;

        // ī�޶��� �ü� ���� ���
        transform.LookAt(player.position );
    }
}
