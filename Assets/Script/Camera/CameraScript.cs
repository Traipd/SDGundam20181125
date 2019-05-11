using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	public Transform Ftransform;
	public bool is_AI_Camera=false;
	// public GameObject gameObject_flag;
	RaycastHit rayback;
	RaycastHit rayforward;
	Vector3 ray_point;
	bool stop_flag=false;
	

	// Use this for initialization
	void Start () {
		
		// Screen.lockCursor=true;//锁定光标
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			stop_flag=!stop_flag;
		}
		
		if(stop_flag)
		{
			Cursor.lockState=CursorLockMode.None;
			Cursor.visible=true;
		}else
		{
			Cursor.lockState=CursorLockMode.Locked;//锁定光标
			Cursor.visible=false;
		}
		
	
		if(Physics.Raycast(Ftransform.position,Ftransform.TransformDirection(Vector3.forward),out rayforward,50))
		{
			ray_point=rayforward.point;
		}else
		{
			ray_point=(Ftransform.position+Ftransform.TransformDirection(Vector3.forward)*50);
		}
      
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		
		avoid_cover();
		
		if(Physics.Raycast(Ftransform.position,Ftransform.TransformDirection(Vector3.forward),out rayforward,50))
		{
			ray_point=rayforward.point;
		}else
		{
			ray_point=(Ftransform.position+Ftransform.TransformDirection(Vector3.forward)*50);
		}
		// Debug.Log("ray_point:"+ray_point);
		   
	}
	///<summary>向后探测避免摄像头被遮挡</summary>
	void avoid_cover()
	{
		Vector3 Fposition=  Ftransform.position;
		Vector3 position=  GetComponent<Transform>().position;	
        bool hit = Physics.Raycast(Fposition,Ftransform.TransformDirection(Vector3.back),out rayback,7);
		if(hit)
		{
            GetComponent<Transform>().position=rayback.point;
		}else
		{
			GetComponent<Transform>().position=Fposition+Ftransform.TransformDirection(Vector3.back)*7;
		}
	}
	///<summary>获取屏幕中心射线的碰撞点坐标</summary>
	public Vector3 getRay_point()
	{
		if(is_AI_Camera)
		{
			return GameObject.FindWithTag("Player").GetComponent<Transform>().position+Vector3.up*1.5f;
		}
		return ray_point;
	}
	///<summary>获取屏幕中心射线的右方向</summary>
	public Vector3 getRight()
	{
		return Ftransform.right;
	}
}
