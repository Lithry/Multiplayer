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
	private NetworkStartPosition[] spawnPoints;

	void Awake(){
		barInside = healthBar.transform.Find("HealthColor").GetComponent<Image>();
		currentHealth = maxHealth;
		//if (isLocalPlayer)
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
	}

	public void ReciveDamage(int dmg){
		if (!isServer)
			return;

		currentHealth -= dmg;
		if (currentHealth <= 0){
			if (gameObject.tag == "Player")
				RpcRespawn();
			else
				Destroy(gameObject);
		}
	}

	public void Heal(int heal){
		currentHealth += heal;
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;
	}

	private void SetHealthAmmount(int currentHealth){
		barInside.fillAmount = (float)((float)currentHealth / (float)maxHealth);
	}

	[ClientRpc]
	void RpcRespawn(){
		Vector3 spawnPoint = Vector3.zero;

		if (spawnPoints != null && spawnPoints.Length > 0){
		    spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
		}

		transform.position = spawnPoint;
		currentHealth = maxHealth;
	}
}
