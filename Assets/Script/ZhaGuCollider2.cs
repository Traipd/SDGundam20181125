using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhaGuCollider2 : MonoBehaviour {
    public Transform camera_transform;
	public float g_inScene=5;//重力参数
	public float speed=3;//速度参数
	public float fly_slider_reflex=1;//飞翔条回复参数
	public float fly_slider_cost=1;//飞翔条消耗参数
	public Slider slider;
	//是否浮空//是否在地面//是否在飞翔
	bool is_fu=false,is_onFloor=false,is_fly=false;
	float w_keytime=0,s_keytime=0,a_keytime=0,d_keytime=0,space_keytime=0;
	// Use this for initialization
	// Transform transform;
	void Start () {
		g_inScene=-g_inScene;
		// transform = this.GetComponent<Transform>();

	}
	void OnCollisionStay(Collision collision){
	    is_onFloor=true;
	}
	void OnCollisionExit(Collision collision){
	    is_onFloor=false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles=new Vector3(0,camera_transform.eulerAngles.y,0);
	}

	void FixedUpdate(){

		if(Input.GetKeyDown(KeyCode.Space)){Debug.Log("down");}
		if(Input.GetKeyUp(KeyCode.Space)){Debug.Log("up");}
	   //rigidbody.velocity=transform.forward.normalized*4;//速度法
		if(Input.anyKey)
		{   
			if(WASD_spacetime_flag()||is_fly)//两次WSAD
			{      
				float h = Input.GetAxis("Horizontal");  
				float v = Input.GetAxis("Vertical");  
				transform.Translate(new Vector3(h*2,0,v*2)*speed*Time.deltaTime);//位移法
				slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);
				transform.Translate(-Vector3.up*g_inScene*0.5f*Time.deltaTime);//飞翔减轻重力
				is_fly=true;
				if(slider.value==0)//飞翔条用尽
				{
					is_fly=false;
				}
			}else{
				//一次WSAD
				if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
				{
					float h = Input.GetAxis("Horizontal");  
					float v = Input.GetAxis("Vertical");  
					transform.Translate(new Vector3(h,0,v)*speed*Time.deltaTime);//位移法
					if (is_onFloor&&!Input.GetKey(KeyCode.Space)){
						slider.value=Mathf.Min(slider.value+0.3f*fly_slider_reflex*Time.fixedDeltaTime,1);
					}	
				}
			}
			
			//空格
			if (Input.GetKey(KeyCode.Space))
			{
				if(is_fu||((Time.fixedTime-space_keytime)>0.018f&&(Time.fixedTime-space_keytime)<0.25f))//浮空
				{
                    is_fu=true;
					slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);
					if(slider.value==0)//飞翔条用尽
					{
						is_fu=false;
					}
				}else{                 //上升
					if(slider.value==0)//飞翔条用尽
					{
						transform.Translate(Vector3.up*g_inScene*Time.deltaTime);
					}else{
						transform.Translate(-Vector3.up*g_inScene*Time.deltaTime);
					    slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);
					}
				}
			}else
			{
				transform.Translate(Vector3.up*g_inScene*Time.deltaTime);
				is_fu=false;
			}
				
		}else
		{   //无按键
			transform.Translate(Vector3.up*g_inScene*Time.deltaTime);
			is_fu=false;
			is_fly=false;
			if(is_onFloor){
				slider.value=Mathf.Min(slider.value+0.3f*fly_slider_reflex*Time.fixedDeltaTime,1);
			}
		}	

        setKeytime();
	}
    bool WASD_spacetime_flag()//按键时间间隔检测
	{   
		float w=0,s=0,a=0,d=0;
		if(Input.GetKey(KeyCode.W))
		{w=Time.fixedTime-w_keytime;}
		if(Input.GetKey(KeyCode.S))
		{s=Time.fixedTime-s_keytime;}
		if(Input.GetKey(KeyCode.A))
		{a=Time.fixedTime-a_keytime;}
		if(Input.GetKey(KeyCode.D))
		{d=Time.fixedTime-d_keytime;}

        if((w>0.018f&&w<0.14f)||(s>0.018f&&s<0.14f)||(a>0.018f&&a<0.14f)||(d>0.018f&&d<0.14f)){
             return true;
		}else{
			return false;
		}	
	}

	void setKeytime(){//记录按键时间
		if(Input.GetKey(KeyCode.W))
		{ w_keytime=Time.fixedTime;}
		if(Input.GetKey(KeyCode.S))
		{ s_keytime=Time.fixedTime;}
		if(Input.GetKey(KeyCode.A))
		{ a_keytime=Time.fixedTime;}
		if(Input.GetKey(KeyCode.D))
		{ d_keytime=Time.fixedTime;}
		if(Input.GetKey(KeyCode.Space))
		{ space_keytime=Time.fixedTime;}
	}
}
