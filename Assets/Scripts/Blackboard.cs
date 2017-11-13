using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour{
	public static Blackboard instance;
	public List<GameObject> players = new List<GameObject>();

	void Awake(){
		instance = this;
	}
}
