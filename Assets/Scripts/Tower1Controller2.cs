using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tower1Controller2 : NetworkBehaviour {
	public GameObject shootPrefab;
	public GameObject aim;
	public GameObject shootTip;
	private GameObject ally;
	private GameObject target;
	public float delay = 3.0f;
	public int distance = 12;
	private float timer;

	void Awake () {
		timer = 0.0f;
	}
	
	void Update () {
		if (isLocalPlayer){
			return;
		}

		timer += Time.deltaTime;

		if (target !=null && Vector3.Distance(transform.position, target.transform.position) < distance && timer > delay){
			Aim(target.transform.position);
			CmdShoot();
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
	}

	void OnTriggerStay2D(Collider2D coll){
		if (target == null && coll.gameObject.tag == "Player1"){
			target = coll.gameObject;
		}
	}
}
