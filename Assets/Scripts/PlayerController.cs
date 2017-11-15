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

	void Awake () {
		trans = transform;
		bodyTrans = trans.Find("Body").transform;
		Blackboard.instance.players.Add(gameObject);
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
		NetworkServer.Spawn(tower);
	}

	[Command]
	private void CmdSpawnTower2(){
		GameObject tower = Instantiate(tower2Prefab, trans.position, Quaternion.Euler(0, 0, 0));
		tower.GetComponent<Tower2Controller>().SetAlly(this.gameObject, this.gameObject.GetComponent<Health>());
		NetworkServer.Spawn(tower);
	}

	public override void OnStartLocalPlayer(){
		if ((Blackboard.instance.players.Count % 2) == 0){
			/*bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 234.0f, 255.0f, 1.0f);
			jewel.GetComponent<SpriteRenderer>().color = new Color(111.0f, 213.0f, 255.0f, 1.0f);
			ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( new Color(111.0f, 213.0f, 255.0f, 1.0f) );*/
			bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			jewel.GetComponent<SpriteRenderer>().color = Color.red;
			ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( Color.red );
		}
		else{
			/*bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = new Color(255.0f, 239.0f, 17.0f, 1.0f);
			jewel.GetComponent<SpriteRenderer>().color = new Color(255.0f, 251.0f, 111.0f, 1.0f);
			ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( new Color(255.0f, 251.0f, 111.0f, 1.0f) );*/
			bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			jewel.GetComponent<SpriteRenderer>().color = Color.blue;
			ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );
		}
		CameraController.instance.Set(trans);
	}
}
