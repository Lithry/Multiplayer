using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	private Transform trans;
	private Transform bodyTrans;
	private Vector3 lookAt;
	private Vector2 direction;
	public GameObject jewel;
	public GameObject ligth;
	public float speed = 6.0f;
	public GameObject gunTip;
	public GameObject magicPrefab;
	public GameObject tower1Prefab;
	public GameObject tower2Prefab;
	private SpriteRenderer sprite;
	private int pId;

	void Awake () {
		trans = transform;
		bodyTrans = trans.Find("Body").transform;
		Blackboard.instance.players.Add(gameObject);
		pId = Blackboard.instance.players.Count - 1;
		if (isServer)
			Blackboard.instance.serverId = pId;
	}
	
	void Update () {
		if (!isLocalPlayer)
			return;

		Movement();

		if (Input.GetMouseButtonDown(0)){
			CmdShoot();
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)){
			CmdSpawnTower1();
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)){
			CmdSpawnTower2();
		}
	}

	private void Movement(){
		lookAt = Input.mousePosition - Camera.main.WorldToScreenPoint(trans.position);
        float angle = (-Mathf.Atan2(lookAt.x, lookAt.y) * Mathf.Rad2Deg);
        bodyTrans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		trans.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
	}

	[Command]
	private void CmdShoot(){
		NetworkServer.Spawn(Instantiate(magicPrefab, gunTip.transform.position, bodyTrans.rotation));
	}

	[Command]
	private void CmdSpawnTower1(){
		GameObject tower = Instantiate(tower1Prefab, trans.position, Quaternion.Euler(0, 0, 0));
		tower.GetComponent<Tower1Controller>().SetAlly(this.gameObject);
		tower.GetComponent<SpriteRenderer>().color = Color.blue;
		NetworkServer.Spawn(tower);
	}

	[Command]
	private void CmdSpawnTower2(){
		GameObject tower = Instantiate(tower2Prefab, trans.position, Quaternion.Euler(0, 0, 0));
		tower.GetComponent<Tower2Controller>().SetAlly(this.gameObject, this.gameObject.GetComponent<Health>());
		tower.GetComponent<SpriteRenderer>().color = Color.blue;
		NetworkServer.Spawn(tower);
	}

	public override void OnStartLocalPlayer(){
		bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
		jewel.GetComponent<SpriteRenderer>().color = Color.blue;
		ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 		settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );
		
		Blackboard.instance.client = gameObject;
		if (isServer)
			Blackboard.instance.server = gameObject;
		CameraController.instance.Set(trans);
	}
}
