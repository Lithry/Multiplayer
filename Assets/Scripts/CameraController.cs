using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
	static public CameraController instance;
	private Transform target;
	private Transform trans;

	void Awake(){
		instance = this;
		trans = transform;
		target = null;
	}

	public void Set(Transform target){
		this.target = target;
	}

	void Update(){
		if (target != null)
			trans.position = new Vector3(target.position.x, target.position.y, -20);
	}

}
