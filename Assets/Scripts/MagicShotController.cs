﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MagicShotController : NetworkBehaviour {
	private Transform trans;
	public float speed = 25.0f;
	public int dmg = 10;

	void Start () {
		trans = transform;
		GetComponent<Rigidbody2D>().velocity = trans.up * speed;
		Destroy(gameObject, 1.5f);
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player1" || coll.gameObject.tag == "Player2" || coll.gameObject.tag == "Tower"){
			coll.transform.GetComponent<Health>().ReciveDamage(dmg);
		}
		Destroy(gameObject);
	}
}