using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tower1Controller : NetworkBehaviour {
	public GameObject shootPrefab;
	public GameObject aim;
	public GameObject shootTip;
	private GameObject ally;
	public float delay = 3.0f;
	public int distance = 12;
	private SpriteRenderer ren;

	private float timer;
	// Use this for initialization
	void Awake () {
		ren = GetComponent<SpriteRenderer>();
		timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer){
			ren.color = Color.blue;
			return;
		}

		timer += Time.deltaTime;

		if (ally != null){
			for (int i = 0; i < Blackboard.instance.players.Count; i++)	{
				if (Blackboard.instance.players[i] != ally && Vector3.Distance(transform.position, Blackboard.instance.players[i].transform.position) < distance && timer > delay){
					Aim(Blackboard.instance.players[i].transform.position);
					CmdShoot();
				}
			}
		}
	}

	private void Aim(Vector3 target){
		Vector3 lookAt = target - transform.position;
        float angle = (-Mathf.Atan2(lookAt.x, lookAt.y) * Mathf.Rad2Deg);
		aim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	[Command]
	private void CmdShoot(){
		NetworkServer.Spawn(Instantiate(shootPrefab, shootTip.transform.position, aim.transform.rotation));
		timer = 0.0f;
	}

	public void SetAlly(GameObject ally){
		this.ally = ally;
		if (ally == Blackboard.instance.client)
			GetComponent<SpriteRenderer>().color = Color.blue;
	}
}
