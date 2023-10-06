using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float rotationSpeed = 1f;
    public float maxSlopeAngle = 40;
    public LayerMask groundMask;
    public float slopeForce = 5f;
    public float slopeForceRayLength = 1f;
    Rigidbody rigidbody;
    Animator animator;
    Vector3 moveDirection;
    // Interaction compoenets
    PlayerInteractor playerInteraction;
    private float slopeAngle;
    private float smoothRotate;
    private Vector3 groundNormal;
    private bool canMove = true;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInteraction = GetComponentInChildren<PlayerInteractor>();
    }


    void Update()
    {
        if (canMove)
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float zInput = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(xInput, 0f, zInput).normalized;
            animator.SetFloat("speed", moveDirection.magnitude);

            Interact();

            if (Input.GetKey(KeyCode.RightBracket))
            {
                TimeManager.Instance.Tick();
            }
        }
    }
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public void Interact()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            playerInteraction.Interact();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerInteraction.ItemInteract();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            playerInteraction.ItemKeep();
        }

    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            if (IsGrounded())
            {

                Vector3 velocity = transform.forward * moveDirection.magnitude * moveSpeed;
                rigidbody.velocity = velocity;

 
                if (moveDirection != Vector3.zero)
                {         
                    Vector3 targetDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
                 

                    transform.rotation = targetRotation;
                }
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, rayDirection, out hit, 1.5f, groundMask))
        {
            Debug.DrawRay(transform.position, rayDirection, Color.red);
            groundNormal = hit.normal;
            return true;
        }
        return false;
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Planet")
        {
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Planet")
        {
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
}