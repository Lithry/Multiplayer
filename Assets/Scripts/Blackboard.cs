using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Blackboard : NetworkBehaviour{
	public static Blackboard instance;
	public List<GameObject> players = new List<GameObject>();
	public GameObject server;
	public GameObject client;


	void Awake(){
		instance = this;
	}
}
