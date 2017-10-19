using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	private Transform trans;
	public GameObject gunPoint;
	public GameObject bullet;
	public Image healthBar;
	private int health;
	

	void Awake () {
		trans = transform;
		health = 100;
	}
	
	void Update () {
		if (!isLocalPlayer)
			return;

		healthBar.fillAmount = health / 100;

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;

		trans.Rotate(0, x, 0);
		trans.Translate(Vector3.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime * 3.0f);
		
		if (Input.GetMouseButtonDown(0)){
			CmdShoot();
		}
		

	}

	[Command]
	private void CmdShoot(){
		NetworkServer.Spawn(Instantiate(bullet, gunPoint.transform.position, gunPoint.transform.rotation));
	}

	public override void OnStartLocalPlayer(){
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
			Destroy(collision.transform.gameObject);
            health -= 10;
			if (health <= 0)
				Destroy(gameObject);
        }
    }
}
