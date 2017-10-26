using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	private Transform trans;
	private Vector3 lookAt;
	private Vector2 direction;
	public float speed = 6.0f;
	public GameObject gunTip;
	public GameObject bulletPrefab;

	void Awake () {
		trans = transform;
	}
	
	void Update () {
		if (!isLocalPlayer)
			return;

		Movement();

		if (Input.GetMouseButtonDown(0)){
			CmdShoot();
		}
	}

	private void Movement(){
		lookAt = Input.mousePosition - Camera.main.WorldToScreenPoint(trans.position);
        float angle = (-Mathf.Atan2(lookAt.x, lookAt.y) * Mathf.Rad2Deg) + 90;
        trans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		trans.Translate(direction.normalized * speed * Time.deltaTime, Space.World);	
	}

	[Command]
	private void CmdShoot(){
		NetworkServer.Spawn(Instantiate(bulletPrefab, gunTip.transform.position, trans.rotation));
	}

	public override void OnStartLocalPlayer(){
		GetComponent<SpriteRenderer>().color = Color.white;
		CameraController.instance.Set(trans);
	}
}
