using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopperController : MonoBehaviour
{
    Rigidbody rigidbody;
    Animator animator;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool("Float", true);
    }
}
