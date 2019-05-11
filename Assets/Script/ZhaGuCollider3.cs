using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhaGuCollider3 : MonoBehaviour {

	public Transform camera_transform;
	public float g_inScene=5;//重力参数
	public float speed=3;//速度参数
	public float fly_slider_reflex=1;//飞翔条回复参数
	public float fly_slider_cost=1;//飞翔条消耗参数
	public Slider slider;
	//是否浮空//是否在地面//是否在飞翔//是否能起飞
	bool is_fu=false,is_onFloor=false,is_fly=false,is_canfly=true;
	float w_keytime=0,s_keytime=0,a_keytime=0,d_keytime=0,space_keytime=0;
	bool w_downing=false,s_downing=false,a_downing=false,d_downing=false,space_downing=false;
	// Use this for initialization
	// Transform transform;
	void Start () {
		g_inScene=-g_inScene;
		// transform = this.GetComponent<Transform>();

	}
	//检测是否在地面
	void OnCollisionStay(Collision collision){
	    
	}
	void OnCollisionExit(Collision collision){
	   
	}

    // 接触结束
    void OnTriggerExit(Collider collider) {
        is_onFloor=false;
    }

    // 接触持续中
    void OnTriggerStay(Collider collider) {
       is_onFloor=true;
    }

	// Update is called once per frame
	void Update () {
		transform.eulerAngles=new Vector3(0,camera_transform.eulerAngles.y,0);
	}

	void FixedUpdate(){	
		//Debug.Log(w_downing+":"+s_downing+":"+a_downing+":"+d_downing);
		    //WSAD
			if(WASD_spacetime_flag()||is_fly)//连击
			{
				float h = Input.GetAxis("Horizontal");  
				float v = Input.GetAxis("Vertical");  
				transform.Translate(new Vector3(2*h,0,2*v)*speed*Time.deltaTime);//位移法
				transform.Translate(Vector3.up*0.5f*-g_inScene*Time.deltaTime);
				slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);
				is_fly=true;
				if(slider.value==0)
				{
                   is_fly=false;
				}
			}
			else if(w_downing==true||s_downing==true||a_downing==true||d_downing==true)//单击
			{                
				float h = Input.GetAxis("Horizontal");  
				float v = Input.GetAxis("Vertical");  
				transform.Translate(new Vector3(h,0,v)*speed*Time.deltaTime);//位移法			
			}
			
			//空格
            if(Space_spacetime_flag()||is_fu)//连击
			{
               is_fu=true;
			   transform.Translate(Vector3.up*-g_inScene*Time.deltaTime);
			   slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);
			   	if(slider.value==0)
				{
                   is_fu=false;
				}
			 
			}else if(space_downing==true)//单击
			{
			   if(slider.value!=0&&is_canfly)
			   {
				 transform.Translate(Vector3.up*2*-g_inScene*Time.deltaTime);
			     slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);
			   }
			}

		  
		setKeytime();//更新按键状态后————————————————————————————————————————————————————————	
				//无按键
				if(w_downing==false&&s_downing==false&&a_downing==false&&d_downing==false&&space_downing==false)
				{
					is_fu=false;
					is_fly=false;				
				}
			
			//必然事件
			if(is_onFloor&&!is_fly&&!is_fu)
			{
				slider.value=Mathf.Min(slider.value+0.3f*fly_slider_reflex*Time.fixedDeltaTime,1);
			}
			transform.Translate(Vector3.up*g_inScene*Time.deltaTime);



		if(slider.value==0&&space_downing==true)
		{
			is_canfly=false;
		}
		if(Input.GetKeyUp(KeyCode.Space))
		{
			is_canfly=true;
		}
		if(w_downing==false&&s_downing==false&&a_downing==false&&d_downing==false)
		{
			is_fly=false;
		}
		if(space_downing==false)
		{
			is_fu=false;
		}
	}
	bool Space_spacetime_flag()//按键按下时间间隔检测
	{
		float s=0;
		if(Input.GetKeyDown(KeyCode.Space))
		{s=Time.fixedTime-space_keytime;  }

		 if(s>0.018f&&s<0.25f){
             return true;
		}else{
			return false;
		}	
	}
    bool WASD_spacetime_flag()//按键时间间隔检测
	{   
		float w=0,s=0,a=0,d=0;
		if(Input.GetKeyDown(KeyCode.W))
		{w=Time.fixedTime-w_keytime;  }
		if(Input.GetKeyDown(KeyCode.S))
		{s=Time.fixedTime-s_keytime;}
		if(Input.GetKeyDown(KeyCode.A))
		{a=Time.fixedTime-a_keytime;}
		if(Input.GetKeyDown(KeyCode.D))
		{d=Time.fixedTime-d_keytime;}

        if((w>0.018f&&w<0.25f)||(s>0.018f&&s<0.25f)||(a>0.018f&&a<0.25f)||(d>0.018f&&d<0.25f)){
             return true;
		}else{
			return false;
		}	
	}

	void setKeytime(){//记录按键时间和状态
		if(Input.GetKeyDown(KeyCode.W))
		{ w_keytime=Time.fixedTime;w_downing=true;}
		if(Input.GetKeyDown(KeyCode.S))
		{ s_keytime=Time.fixedTime;s_downing=true;}
		if(Input.GetKeyDown(KeyCode.A))
		{ a_keytime=Time.fixedTime;a_downing=true;}
		if(Input.GetKeyDown(KeyCode.D))
		{ d_keytime=Time.fixedTime;d_downing=true;}
		if(Input.GetKeyDown(KeyCode.Space))
		{ space_keytime=Time.fixedTime;space_downing=true;}

		if(Input.GetKeyUp(KeyCode.W))
		{ w_downing=false;}
		if(Input.GetKeyUp(KeyCode.S))
		{s_downing=false;}
		if(Input.GetKeyUp(KeyCode.A))
		{ a_downing=false;}
		if(Input.GetKeyUp(KeyCode.D))
		{ d_downing=false;}
		if(Input.GetKeyUp(KeyCode.Space))
		{ space_downing=false;}
	}

}
