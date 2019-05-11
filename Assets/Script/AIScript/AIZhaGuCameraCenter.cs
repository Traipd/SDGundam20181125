using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZhaGuCameraCenter : MonoBehaviour {

	GameObject gObj;
	// Use this for initialization
	void Start () {
		gObj=GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(gameObject==null)
		{
			gObj=GameObject.FindWithTag("Player");
		}
		transform.LookAt(gObj.GetComponent<Transform>().position);
	}
}
