using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ZhaGuAni2 : MonoBehaviour {
    public Animator ani;
	public Slider slider;
	//是否在地面//是否在飞翔
	bool is_onFloor=false,is_fly=false;

   //按键时间
	float w_keytime=0,s_keytime=0,a_keytime=0,d_keytime=0,space_keytime=0;

   //检测是否在地面
	void OnCollisionStay(Collision collision){
	    is_onFloor=true;
	}
	void OnCollisionExit(Collision collision){
	    is_onFloor=false;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.anyKey){    
            if(WASD_spacetime_flag()||is_fly)//两次WSAD
			{
                ani.SetBool("walk",false);
				ani.SetBool("fly",true);
				float h = Input.GetAxis("Horizontal");  
				float v = Input.GetAxis("Vertical");  
				if(h==0){h=0.1f;}
				if(v==0){v=0.1f;}
				ani.SetFloat("fly_lr", h, 0.01f, Time.deltaTime);  
				ani.SetFloat("fly_fb", v, 0.01f, Time.deltaTime);
				is_fly=true;
				if(slider.value==0)//飞翔条用尽
				{
					is_fly=false;
					ani.SetBool("fly",false);
				}
			}else{
				//一次WSAD
				if(!Input.GetKey(KeyCode.Space)&&is_onFloor
				&&(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)))
				{  
					ani.SetBool("walk",true);
					ani.SetBool("fly",false);
					float h = Input.GetAxis("Horizontal");  
					float v = Input.GetAxis("Vertical");  
					if(h==0f){h=0.1f;}
					if(v==0){v=0.1f;}
					ani.SetFloat("lr", h, 0.01f, Time.deltaTime);  
					ani.SetFloat("fb", v, 0.01f, Time.deltaTime);	
				}
				if(Input.GetKey(KeyCode.Space))//空格
				{ ani.SetBool("walk",false); 
				  ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
				}
			}
            //空格
			if(Input.GetKey(KeyCode.Space)&&!(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)))
			{   is_fly=false;
				ani.SetBool("fly",false);
			    ani.SetBool("walk",false); 
				ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);

			}

			if(!is_onFloor){
               ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
			}

		}else{
			//无按键
			is_fly=false;
			ani.SetBool("fly",false);
			ani.SetBool("walk",false);
			if(is_onFloor)
			{
				ani.SetFloat("wait_up",0, 0.02f, Time.deltaTime);
			}else{
				ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
			}
			ani.SetFloat("lr", 0, 0.01f, Time.deltaTime);  
			ani.SetFloat("fb", 0, 0.01f, Time.deltaTime); 
			ani.SetFloat("fly_lr",0);
			ani.SetFloat("fly_fb",0);
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
