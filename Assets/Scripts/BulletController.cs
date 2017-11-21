using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {
	private Transform trans;
	public float speed = 25.0f;
	public int dmg = 10;

	void Start () {
		trans = transform;
		GetComponent<Rigidbody2D>().velocity = trans.right * speed;
		Destroy(gameObject, 1.5f);
	}

	void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player"){
			coll.transform.parent.GetComponent<Health>().ReciveDamage(dmg);
		}
	Destroy(gameObject);
    }
}
