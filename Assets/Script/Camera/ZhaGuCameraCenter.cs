using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhaGuCameraCenter : MonoBehaviour {

    // Transform transform;
	Vector3 angle;
	GameObject gObj;
	public float mouse_speed;//鼠标灵敏度
	public bool is_AI_Camera=false;
	bool stop_flag=false;
	// Use this for initialization
	void Start () {
		// transform=this.GetComponent<Transform>();
		angle=transform.eulerAngles;
		gObj=GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {//关于摄像机的位置信息，使用LateUpdate可以进一步减少画面卡顿
		if(gObj==null||!gObj.activeSelf)
		{
			gObj=GameObject.FindWithTag("Player");
		}
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			stop_flag=!stop_flag;
		}

		if(!stop_flag)
		{
			if(is_AI_Camera)
			{
				if(gameObject==null)
				{
					gObj=GameObject.FindWithTag("Player");
				}
				transform.LookAt(gObj.GetComponent<Transform>().position);
			}else
			{
				if(!Input.GetKey(KeyCode.Mouse2))
				{
					float mouseX=Input.GetAxis("Mouse X");
					float mouseY=Input.GetAxis("Mouse Y");

					angle.x-=mouseY*mouse_speed;
					angle.y+=mouseX*mouse_speed;

					if(angle.x>89){//限制角度
						angle.x=89;
					}
					if(angle.x<-89){
						angle.x=-89;
					}

					transform.eulerAngles=angle;//变换角度	
				}
			}
		}
			

		
	}
}
