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

	

	void Start () {
		trans = transform;
		bodyTrans = trans.Find("Body").transform;
		wins = GameObject.Find("WinText").GetComponent<Text>();
		lost = GameObject.Find("LostText").GetComponent<Text>();
		tower1d = GameObject.Find("Tower1Text").GetComponent<Text>();
		tower2d = GameObject.Find("Tower2Text").GetComponent<Text>();
		tower1 = 10;
		tower2 = 5;
		//Blackboard.instance.player1Wins = 0;
		//Blackboard.instance.player2Wins = 0;
		rb = GetComponent<Rigidbody2D>();
		
		if (isServer){
			if (!isLocalPlayer){
				bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
				jewel.GetComponent<SpriteRenderer>().color = Color.blue;
				ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 				settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );
			}
		}
	}
	
	void Update () {
		if (!isLocalPlayer)
			return;

		/*if (pId == 1){
			wins.text = "Wins: " + Blackboard.instance.player1Wins.ToString();
			lost.text = "Lost: " + Blackboard.instance.player2Wins.ToString();
		}
		else{
			wins.text = "Wins: " + Blackboard.instance.player2Wins.ToString();
			lost.text = "Lost: " + Blackboard.instance.player1Wins.ToString();
		}*/

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
		if (id == 1){
			tower = Instantiate(tower1Prefab1, gunTip.transform.position, Quaternion.Euler(0, 0, 0));
			tower.GetComponent<Tower1Controller>().SetAlly(this.gameObject);
		}
		else{
			tower = Instantiate(tower1Prefab2, gunTip.transform.position, Quaternion.Euler(0, 0, 0));
			tower.GetComponent<Tower1Controller2>().SetAlly(this.gameObject);
		}
		NetworkServer.Spawn(tower);
	}

	[Command]
	private void CmdSpawnTower2(int id){
		GameObject tower;
		if (id == 1)
			tower = Instantiate(tower2Prefab1, gunTip.transform.position, Quaternion.Euler(0, 0, 0));
		else
			tower = Instantiate(tower2Prefab2, gunTip.transform.position, Quaternion.Euler(0, 0, 0));
		tower.GetComponent<Tower2Controller>().SetAlly(this.gameObject, this.gameObject.GetComponent<Health>());
		NetworkServer.Spawn(tower);
	}

	[Command]
	private void CmdChangeTag(){
		gameObject.tag = "Player2";
	}
	
	public override void OnStartLocalPlayer(){
		trans = transform;
		bodyTrans = trans.Find("Body").transform;
		CameraController.instance.Set(trans);
		health = GetComponent<Health>();
		
		if (isServer){
			pId = 1;
			gameObject.tag = "Player1";
		}
		else{
			pId = 2;
			gameObject.tag = "Player2";
			CmdChangeTag();
		}

		if (pId == 2){
			bodyTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			jewel.GetComponent<SpriteRenderer>().color = Color.blue;
			ParticleSystem.MainModule settings = ligth.GetComponent<ParticleSystem>().main;
 			settings.startColor = new ParticleSystem.MinMaxGradient( Color.blue );
		}	
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
