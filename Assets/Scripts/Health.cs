using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
	public int maxHealth = 100;
	public GameObject healthBar;

	[SyncVar(hook = "SetHealthAmmount")]
	private int currentHealth;
	private Image barInside;

	void Awake(){
		barInside = healthBar.transform.Find("HealthColor").GetComponent<Image>();
		currentHealth = maxHealth;
	}

	public void ReciveDamage(int dmg){
		Debug.Log("Dmg: " + dmg);
		if (!isServer)
			return;

		currentHealth -= dmg;
		Debug.Log("Current Health: " + currentHealth);
		if (currentHealth <= 0)
			RpcRespawn();
	}

	private void SetHealthAmmount(int currentHealth){
		barInside.fillAmount = (float)currentHealth / (float)maxHealth;
	}

	[ClientRpc]
	void RpcRespawn(){
		Debug.Log("Entro en Respawn");
		transform.position = Vector3.zero;
		currentHealth = maxHealth;
	}
}
