using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZhaGuAni3 : MonoBehaviour {
	public Animator ani;	
	public Slider slider;

	public GameObject[] fly_fire;
	//是否浮空//是否在地面//是否在飞翔//是否能起飞
	bool is_fu=false,is_onFloor=false,is_fly=false,is_canfly=true;
	float w_keytime=0,s_keytime=0,a_keytime=0,d_keytime=0,space_keytime=0;
	bool w_downing=false,s_downing=false,a_downing=false,d_downing=false,space_downing=false;
	// Use this for initialization
	void Start () {
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

	}

	void FixedUpdate(){	
		    //WSAD
			if(WASD_spacetime_flag()||is_fly)//连击
			{
				float h = Input.GetAxis("Horizontal");  
				float v = Input.GetAxis("Vertical");  
			    ani.SetBool("walk",false);
				ani.SetBool("fly",true);
				if(h==0){h=0.1f;}
				if(v==0){v=0.1f;}
				ani.SetFloat("fly_lr", h, 0.01f, Time.deltaTime);  
				ani.SetFloat("fly_fb", v, 0.01f, Time.deltaTime);
				is_fly=true;
				if(slider.value<0.005f)
				{
                   is_fly=false;
				   ani.SetBool("fly",false);
				}

				 setFlyfireVisible();
			}
			else if(w_downing==true||s_downing==true||a_downing==true||d_downing==true)//单击
			{      
				if(is_onFloor==true)
				{
					float h = Input.GetAxis("Horizontal");  
					float v = Input.GetAxis("Vertical");  
					ani.SetBool("walk",true);
					ani.SetBool("fly",false);
					if(h==0f){h=0.1f;}
					if(v==0){v=0.1f;}
					ani.SetFloat("lr", h, 0.01f, Time.deltaTime);  
					ani.SetFloat("fb", v, 0.01f, Time.deltaTime);	

					setFlyfireUnVisible();
				}else
				{
					 setFlyfireVisible();   
				}   
				     
			
			}
			
			//空格
            if(Space_spacetime_flag()||is_fu)//连击
			{
               is_fu=true;
	
			   	if(slider.value<0.005f)
				{
                   is_fu=false;
				}
			 
			   setFlyfireVisible();
			}else if(space_downing==true)//单击
			{
			   if(slider.value>=0.005f&&is_canfly)
			   {
				  ani.SetBool("walk",false); 
				  ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
			   }
			   else if(w_downing==false&&s_downing==false&&a_downing==false&&d_downing==false){
				   if(is_onFloor==true){
					    ani.SetBool("fly",false);
						ani.SetBool("walk",false);
						ani.SetFloat("wait_up",0, 0.02f, Time.deltaTime);
				   }
			   }
			    setFlyfireVisible();
			}

	
		
		setKeytime();//更新按键状态后————————————————————————————————————————————————————————
				   //无按键
			if(w_downing==false&&s_downing==false&&a_downing==false&&d_downing==false&&space_downing==false)
	        {
				is_fu=false;
				is_fly=false;
				ani.SetBool("fly",false);
				ani.SetBool("walk",false);
				if(is_onFloor)
				{
					ani.SetFloat("wait_up",0, 0.02f, Time.deltaTime);

					setFlyfireUnVisible();	
				}else{
					ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);

					setFlyfireVisible();
				}
				ani.SetFloat("lr", 0, 0.01f, Time.deltaTime);  
				ani.SetFloat("fb", 0, 0.01f, Time.deltaTime); 
				ani.SetFloat("fly_lr",0);
				ani.SetFloat("fly_fb",0);

							
			}
           
        //必然事件
		if(is_onFloor&&!is_fly&&!is_fu)
		{
			
		}
		if(!is_onFloor&&!is_fly)
		{
			ani.SetBool("fly",false);
			ani.SetBool("walk",false); 
		    ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
		}
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

    void setFlyfireVisible()
	{
		for(int i=0;i<fly_fire.Length;i++)
		{
			fly_fire[i].GetComponent<SkinnedMeshRenderer>().enabled=true;
		}
	}
	void setFlyfireUnVisible()
	{
		for(int i=0;i<fly_fire.Length;i++)
		{
			fly_fire[i].GetComponent<SkinnedMeshRenderer>().enabled=false;
		}
	}
}
