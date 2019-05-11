using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperUIScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.H))
		{
			GetComponent<CanvasGroup>().alpha=0.8f;
		}else
		{
			GetComponent<CanvasGroup>().alpha=0;
		}
		
	}
}
