using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tower1Controller : NetworkBehaviour {
	public GameObject shootPrefab;
	public GameObject aim;
	public GameObject shootTip;
	
	private float timer;
	// Use this for initialization
	void Awake () {
		timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		for (int i = 0; i < Blackboard.instance.players.Count; i++)	{
			if (Vector3.Distance(transform.position, Blackboard.instance.players[i].transform.position) < 15 && timer > 3.0f){
				Vector3 lookAt = Blackboard.instance.players[i].transform.position - transform.position;
        		float angle = (-Mathf.Atan2(lookAt.x, lookAt.y) * Mathf.Rad2Deg);
				aim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				CmdShoot(Blackboard.instance.players[i].transform.position);
			}
		}
	}

	[Command]
	private void CmdShoot(Vector3 enemiPos){
		NetworkServer.Spawn(Instantiate(shootPrefab, shootTip.transform.position, aim.transform.rotation));
		timer = 0.0f;
	}
}
