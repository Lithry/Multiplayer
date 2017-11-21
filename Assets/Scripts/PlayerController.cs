using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {
	private Transform trans;
	private Transform bodyTrans;
	private Vector3 lookAt;
	private Vector2 direction;
	public GameObject jewel;
	public GameObject ligth;
	public float speed = 400.0f;
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
	private Text wins;
	private Text lost;
	private Text tower1d;
	private Text tower2d;

	

	void Awake () {
		trans = transform;
		bodyTrans = trans.Find("Body").transform;
		wins = GameObject.Find("WinText").GetComponent<Text>();
		lost = GameObject.Find("LostText").GetComponent<Text>();
		tower1d = GameObject.Find("Tower1Text").GetComponent<Text>();
		tower2d = GameObject.Find("Tower2Text").GetComponent<Text>();
		tower1 = 10;
		tower2 = 5;
		rb = GetComponent<Rigidbody2D>();	
	}
	
	void Update () {
		if (!isLocalPlayer)
			return;

		if (pId == 1){
			wins.text = "Wins: " + Blackboard.instance.player1Wins.ToString();
			lost.text = "Lost: " + Blackboard.instance.player2Wins.ToString();
		}
		else{
			wins.text = "Wins: " + Blackboard.instance.player2Wins.ToString();
			lost.text = "Lost: " + Blackboard.instance.player1Wins.ToString();
		}

		tower1d.text = "Tower1: " + tower1.ToString();
		tower2d.text = "Tower2: " + tower2.ToString();

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
		for (int i = 0; i < Blackboard.instance.players.Count; i++){
			if (Blackboard.instance.players[i] != null && Blackboard.instance.players[i].GetComponent<PlayerController>().GetId() == 2){
				Blackboard.instance.players[i].GetComponent<PlayerController>().GetBody().gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
				Blackboard.instance.players[i].GetComponent<PlayerController>().GetJewel().GetComponent<SpriteRenderer>().color = Color.blue;
				ParticleSystem.MainModule settings = Blackboard.instance.players[i].GetComponent<PlayerController>().GetLigth().GetComponent<ParticleSystem>().main;
 				settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );

				Blackboard.instance.players[i].GetComponent<PlayerController>().GetHealth().RpcSpawn(2);
			}
		}
	}

	public override void OnStartLocalPlayer(){
		health = GetComponent<Health>();
		Blackboard.instance.players.Add(gameObject);
		
		if (isServer){
			pId = 1;
		}
		else{
			pId = 2;
		}
		Blackboard.instance.player1Wins = 0;
		Blackboard.instance.player2Wins = 0;

		if (pId == 2){
			bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			jewel.GetComponent<SpriteRenderer>().color = Color.blue;
			ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );
			int id = Blackboard.instance.players.Count - 1;
			CmdChangePlayerColor(id);
		}
		

		CameraController.instance.Set(trans);
		
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

	public Health GetHealth(){
		return health;
	}

	public int GetId(){
		return pId;
	}

	public void RestarTowers(){
		tower1 = 10;
		tower2 = 5;
	}
}
