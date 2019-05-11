using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class is_Onfloor : MonoBehaviour {

	public bool flag=false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerExit(Collider collider) {
        flag=false;
    }

    // 接触持续中
    void OnTriggerStay(Collider collider) {
       flag=true;
    }
}
