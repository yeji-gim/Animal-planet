using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{

	GravityAttractor planet;
	Rigidbody rigidbody;

	void Awake()
	{
		planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}
	void FixedUpdate()
	{
		if (planet != null)
		{
			planet.Attract(rigidbody);
		}
	}
}
