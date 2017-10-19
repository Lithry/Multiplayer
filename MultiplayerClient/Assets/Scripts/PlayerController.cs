using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Transform trans;
	private Animator animator;
	// Use this for initialization
	void Awake () {
		trans = transform;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//var y = Input.GetAxisRaw("Horizontral") * Time.deltaTime * 150.0f;

		//trans.Rotate(0, y, 0);
		if (Input.GetAxisRaw("Vertical") != 0)
			animator.SetBool("Walk", true);
		else
			animator.SetBool("Walk", false);
			
		trans.Translate(Vector2.up * Input.GetAxisRaw("Vertical") * 3.0f * Time.deltaTime);
		var x = -Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;

		trans.Rotate(0, 0, x);
	}
}
