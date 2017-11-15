using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealEffect : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 1.0f);
	}
}
