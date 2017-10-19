using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {
	private Transform trans;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * 20.0f;
		Destroy(gameObject, 2);
	}
}
