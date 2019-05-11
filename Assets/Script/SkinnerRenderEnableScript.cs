using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnerRenderEnableScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<SkinnedMeshRenderer>().enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
