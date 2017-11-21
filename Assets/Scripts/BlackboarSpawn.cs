using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlackboarSpawn : NetworkBehaviour {
	public GameObject blackboard;
	public override void OnStartServer(){
		NetworkServer.Spawn(Instantiate(blackboard));
	}
}
