﻿using System.Collections;
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
	private NetworkStartPosition spawnPoint1;
	private NetworkStartPosition spawnPoint2;
	private PlayerController player;
	void Awake(){
		barInside = healthBar.transform.Find("HealthColor").GetComponent<Image>();
		currentHealth = maxHealth;
		if (gameObject.tag == "Player"){
			player = GetComponent<PlayerController>();
		}
        NetworkStartPosition[] spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			if (i == 0)
				spawnPoint1 = spawnPoints[i];
			else
				spawnPoint2 = spawnPoints[i];

		}
	}

	public void ReciveDamage(int dmg){
		if (!isServer)
			return;

		currentHealth -= dmg;
		if (currentHealth <= 0){
			if (gameObject.tag == "Player1")
				RpcRespawn(1);
			else if (gameObject.tag == "Player2")
				RpcRespawn(2);
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
	void RpcRespawn(int id){
		Vector3 spawnPoint = Vector3.zero;

		if (id == 1){
			spawnPoint = spawnPoint1.transform.position;
			GameObject.FindWithTag("Player2").GetComponent<PlayerController>().AddWin();
			GameObject.FindWithTag("Player1").GetComponent<PlayerController>().AddLost();
		}
		else{
			spawnPoint = spawnPoint2.transform.position;
			GameObject.FindWithTag("Player1").GetComponent<PlayerController>().AddWin();
			GameObject.FindWithTag("Player2").GetComponent<PlayerController>().AddLost();
		}

		transform.position = spawnPoint;
		currentHealth = maxHealth;
	}

	[ClientRpc]
	public void RpcSpawn(int id){
		Vector3 spawnPoint = Vector3.zero;

		if (id == 1){
			spawnPoint = spawnPoint1.transform.position;
		}
		else
			spawnPoint = spawnPoint2.transform.position;

		transform.position = spawnPoint;
		currentHealth = maxHealth;
	}
}
