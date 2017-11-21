using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Blackboard : NetworkBehaviour{
	public static Blackboard instance;
	public List<GameObject> players = new List<GameObject>();
	public GameObject server;
	public GameObject client;
	public int player1Wins;
	public int player2Wins;
	private int conn = 2;


	void Awake(){
		instance = this;
		conn = 0;
		for (int i = 0; i < NetworkServer.connections.Count; i++)
		{
			if (NetworkServer.connections[i] != null)
				conn++;
		}
	}

	/*void Update(){
		int a = 0;
		for (int i = 0; i < NetworkServer.connections.Count; i++)
		{
			if (NetworkServer.connections[i] != null)
				a++;
		}

		if (a < conn){
			ResetPlayers();
			conn = a;
		}
	}*/

	public void ResetPlayers(){
		players.Clear();
		GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < p.Length; i++)
		{
			players.Add(p[i]);
		}
	}
}
