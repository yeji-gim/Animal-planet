using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float rotSpeed = 20f;
    void Update()
    {
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
        // Time.deltaTime�� ȭ���� �� �������̴� �ð��� ���.
    }
}
