using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraCenterScript : MonoBehaviour {

	 // Transform transform;
	Vector3 angle;
	
	public float mouse_speed;//鼠标灵敏度
	public Camera freecamera;
	// Use this for initialization
	void Start () {
		// transform=this.GetComponent<Transform>();
		angle=transform.eulerAngles;
		freecamera.depth=-2;
	}
	
	// Update is called once per frame
	void LateUpdate () {//关于摄像机的位置信息，使用LateUpdate可以进一步减少画面卡顿
		
		if(Input.GetKey(KeyCode.Mouse2))
		{
			freecamera.depth=2;
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
		}else
		{
			freecamera.depth=-2;
		}
			
		
	}
}
