using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
	public float gravity = -9.8f;
	private Quaternion targetRotation;


	public void Attract(Rigidbody body)
	{
		
		Vector3 localUp = body.transform.up;

		body.AddForce(gravityUp(body) * gravity);
		body.rotation = Quaternion.FromToRotation(localUp, gravityUp(body)) * body.rotation;
	}


	public Vector3 gravityUp(Rigidbody body)
    {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		return gravityUp;
	}
}
