using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		transform.LookAt(Camera.main.transform.position);
	}
}
