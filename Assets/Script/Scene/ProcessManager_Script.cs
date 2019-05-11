using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessManager_Script : MonoBehaviour {

	bool stop_flag=false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			stop_flag=!stop_flag;
		}
		if(stop_flag)
		{
			Time.timeScale=0;
		}else
		{
			Time.timeScale=1;
		}
	}
}
