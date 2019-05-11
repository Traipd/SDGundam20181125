using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhaGuCollider : MonoBehaviour {
	public Transform camera_transform;
	public float g_inScene;
	public float speed;
	
	// Use this for initialization
	// Transform transform;
	void Start () {
		g_inScene=-g_inScene;
		// transform = this.GetComponent<Transform>();

	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles=new Vector3(0,camera_transform.eulerAngles.y,0);
   //     Debug.Log("x:"+camera_transform.forward.x+"y:"+camera_transform.forward.y
   //	  +"z:"+camera_transform.forward.z);
	}

	void FixedUpdate(){
	   //rigidbody.velocity=transform.forward.normalized*4;//速度法
		if(Input.anyKey)
		{
			if(Input.GetKey(KeyCode.W))
			{
			transform.Translate(Vector3.forward*speed*Time.deltaTime);//位移法
			}
			else if(Input.GetKey(KeyCode.S))
			{
			transform.Translate(-Vector3.forward*speed*Time.deltaTime);
			}
			else if(Input.GetKey(KeyCode.D))
			{
			transform.Translate(Vector3.right*speed*Time.deltaTime);
			}
			else if(Input.GetKey(KeyCode.A))
			{
			transform.Translate(-Vector3.right*speed*Time.deltaTime);
			}
			
			if (Input.GetKey(KeyCode.Space))
			{
				transform.Translate(-Vector3.up*g_inScene*Time.deltaTime);
			}else
			{
				transform.Translate(Vector3.up*g_inScene*Time.deltaTime);
			}
				
	          
		}else
		{
			transform.Translate(Vector3.up*g_inScene*Time.deltaTime);
		}	

	}
}
