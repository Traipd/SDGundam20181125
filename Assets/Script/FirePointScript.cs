using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointScript : MonoBehaviour {

	// Use this for initialization
	public GameObject ZiDan;
	void Start () {
		
	}
	
	float timedelay=0;
	// Update is called once per frame
	void Update () {
       timedelay+=Time.deltaTime;
		
		if(ZiDan!=null&&timedelay>2&&Input.GetMouseButtonDown(0))
		{    timedelay=0;

			// GameObject go=GameObject.Instantiate<GameObject>(ZiDan,transform.position,transform.rotation);
			GameObject.Instantiate<GameObject>(ZiDan,transform.position,transform.rotation);
			// go.transform.SetParent(false);=this.GetComponent<Transform>();
		
		}
	}
}
