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
	public GameObject tower1Prefab1;
	public GameObject tower1Prefab2;
	public GameObject tower2Prefab1;
	public GameObject tower2Prefab2;
	private SpriteRenderer sprite;
	private int pId;
	private Health health;
	private int tower1;
	private int tower2;
	private Rigidbody2D rb;

	void Awake () {
		trans = transform;
		bodyTrans = trans.Find("Body").transform;
		tower1 = 10;
		tower2 = 5;
		rb = GetComponent<Rigidbody2D>();
		Blackboard.instance.players.Add(gameObject);	
	}
	
	void Update () {
		if (!isLocalPlayer)
			return;

		Movement();

		if (Input.GetMouseButtonDown(0)){
			CmdShoot();
		}

		if (Input.GetKeyDown(KeyCode.Alpha1) && tower1 > 0){
			CmdSpawnTower1(pId);
			tower1--;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2) && tower2 > 0){
			CmdSpawnTower2(pId);
			tower2--;
		}
	}

	private void Movement(){
		lookAt = Input.mousePosition - Camera.main.WorldToScreenPoint(trans.position);
        float angle = (-Mathf.Atan2(lookAt.x, lookAt.y) * Mathf.Rad2Deg);
        bodyTrans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		rb.velocity = (direction * speed * Time.deltaTime);
		//trans.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
	}

	[Command]
	private void CmdShoot(){
		NetworkServer.Spawn(Instantiate(magicPrefab, gunTip.transform.position, bodyTrans.rotation));
	}

	[Command]
	private void CmdSpawnTower1(int id){
		GameObject tower;
		if (id == 1)
			tower = Instantiate(tower1Prefab1, trans.position, Quaternion.Euler(0, 0, 0));
		else
			tower = Instantiate(tower1Prefab2, trans.position, Quaternion.Euler(0, 0, 0));
		tower.GetComponent<Tower1Controller>().SetAlly(this.gameObject);
		NetworkServer.Spawn(tower);
	}

	[Command]
	private void CmdSpawnTower2(int id){
		GameObject tower;
		if (id == 1)
			tower = Instantiate(tower2Prefab1, trans.position, Quaternion.Euler(0, 0, 0));
		else
			tower = Instantiate(tower2Prefab2, trans.position, Quaternion.Euler(0, 0, 0));
		tower.GetComponent<Tower2Controller>().SetAlly(this.gameObject, this.gameObject.GetComponent<Health>());
		NetworkServer.Spawn(tower);
	}

	[Command]
	private void CmdChangePlayerColor(int id){
			Blackboard.instance.players[id].GetComponent<PlayerController>().GetBody().gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			Blackboard.instance.players[id].GetComponent<PlayerController>().GetJewel().GetComponent<SpriteRenderer>().color = Color.blue;
			ParticleSystem.MainModule settings = Blackboard.instance.players[id].GetComponent<PlayerController>().GetLigth().GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );
	}

	public override void OnStartLocalPlayer(){
		if (isServer){
			pId = 1;
		}
		else
			pId = 2;

		if (pId == 2){
			bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			jewel.GetComponent<SpriteRenderer>().color = Color.blue;
			ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );
			int id = Blackboard.instance.players.Count - 1;
			CmdChangePlayerColor(id);
		}
		
		health = GetComponent<Health>();

		CameraController.instance.Set(trans);
		health.RpcSpawn(pId);
	}

	public Transform GetBody(){
		return bodyTrans;
	}

	public GameObject GetJewel(){
		return jewel;
	}

	public GameObject GetLigth(){
		return ligth;
	}

	public int GetId(){
		return pId;
	}

	public void RestarTowers(){
		tower1 = 10;
		tower2 = 5;
	}
}
