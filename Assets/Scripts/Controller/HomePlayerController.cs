using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePlayerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float rotationSpeed = 1f;
    public float maxSlopeAngle = 40;

    Vector3 moveDirection;
    Rigidbody rigidbody;
    Animator animator;
    private float smoothRotate;
    private Vector3 groundNormal;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }


    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(xInput, 0f, zInput).normalized;
        animator.SetFloat("speed", moveDirection.magnitude);

        if (Input.GetKey(KeyCode.RightBracket))
        {
            TimeManager.Instance.Tick();
        }


    }

    void FixedUpdate()
    {
         Vector3 velocity = transform.forward * moveDirection.magnitude * _speed;
         rigidbody.velocity = velocity;

            
         if (moveDirection != Vector3.zero )
         {
             smoothRotate = Mathf.Lerp(smoothRotate, rotationSpeed, Time.deltaTime * 10f);
             Quaternion targetRotation = Quaternion.LookRotation(moveDirection, transform.up);
             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
         }
    }
}