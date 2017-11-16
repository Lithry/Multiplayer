using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tower2Controller : NetworkBehaviour {
	private GameObject ally;
	private Health allyHealth;
	private float timer;
	public int power = 10;
	public float delay = 3.0f;
	public int distance = 12;
	public GameObject healEffect;
	private SpriteRenderer ren;

	void Awake () {
		ren = GetComponent<SpriteRenderer>();

		timer = 0.0f;
	}
	
	void Update () {
		if (isLocalPlayer){
			ren.color = Color.blue;
			return;
		}

		timer += Time.deltaTime;

		if (ally != null && Vector3.Distance(transform.position, ally.transform.position) < distance && timer > delay){
			CmdHeal();
		}
	}

	[Command]
	private void CmdHeal(){
		allyHealth.Heal(power);
		NetworkServer.Spawn(Instantiate(healEffect, ally.transform.position, Quaternion.Euler(0, 0, 0)));
		timer = 0.0f;
	}

	public void SetAlly(GameObject ally, Health allyHealth){
		this.ally = ally;
		this.allyHealth = allyHealth;
		if (ally == Blackboard.instance.client)
			GetComponent<SpriteRenderer>().color = Color.blue;
	}
}