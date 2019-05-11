using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhaGuAni : MonoBehaviour {
    public Animator ani;
	bool is_onFloor=false;

	KeyCode keytab;
	float keytime=0;
    bool if_spacedown=false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnCollisionStay(Collision collision){
	    is_onFloor=true;
	}
	void OnCollisionExit(Collision collision){
	    is_onFloor=false;
	}
	void FixedUpdate () {
		// Debug.Log(Time.fixedTime-keytime);
		//空格键 
		if(Input.GetKey(KeyCode.Space)){
			if( Time.fixedTime-keytime<0.3f&&Time.fixedTime-keytime>0.018f&&(Input.GetKey(keytab)))
			{
                ani.SetBool("walk",false);
				ani.SetBool("fly",true);
			}else{
				setKeytab();
				if_spacedown=true;
				ani.SetBool("walk",false);
				ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
			}
            
		}
		//WASD
		else if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)){
            if( Time.fixedTime-keytime<0.3f&&Time.fixedTime-keytime>0.018f&&(Input.GetKey(keytab)))
			{
                ani.SetBool("walk",false);
				ani.SetBool("fly",true);
			}else{
				setKeytab();
				if(!if_spacedown&&is_onFloor){
					ani.SetBool("walk",true);
					float h = Input.GetAxis("Horizontal");  
					float v = Input.GetAxis("Vertical");  
					ani.SetFloat("lr", h, 0.01f, Time.deltaTime);  
					ani.SetFloat("fb", v, 0.01f, Time.deltaTime);
				}else if(!is_onFloor){
					ani.SetBool("walk",false);
					ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
				}	
			}
            
		
		}
		//无按键
		else{
	
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
		
		
		


        if(Input.GetKeyUp(KeyCode.Space))
		{
			if_spacedown=false;
		}
		// if(Input.anyKey)
		// {
        //     if( Time.fixedTime-keytime<0.5f&&Time.fixedTime-keytime>0.017f&&(Input.GetKey(keytab)))
		// 	{
        //          ani.SetBool("walk",false);
		// 		  ani.SetBool("fly",true);
		// 		  if(Input.GetKey(KeyCode.W))
        //             {ani.SetFloat("fly_fb",1, 0.02f, Time.deltaTime);}
		// 			else if(Input.GetKey(KeyCode.S))
		// 			{ani.SetFloat("fly_fb",-1, 0.02f, Time.deltaTime);}
		// 			else if(Input.GetKey(KeyCode.A))
		// 			{ani.SetFloat("fly_lr",-1, 0.02f, Time.deltaTime);}
		// 			else if(Input.GetKey(KeyCode.D))
		// 			{ani.SetFloat("fly_lr",1, 0.02f, Time.deltaTime);}
		// 	}else{
		// 		if(!is_onFloor)
		// 		{   
		// 			ani.SetBool("walk",false);
		// 			ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
		// 		}else{
		// 			if(Input.GetKey(KeyCode.W))
        //             {keytime=Time.fixedTime;keytab=KeyCode.W;}
		// 			else if(Input.GetKey(KeyCode.S))
		// 			{keytime=Time.fixedTime;keytab=KeyCode.S;}
		// 			else if(Input.GetKey(KeyCode.A))
		// 			{keytime=Time.fixedTime;keytab=KeyCode.A;}
		// 			else if(Input.GetKey(KeyCode.D))
		// 			{keytime=Time.fixedTime;keytab=KeyCode.D;}

		// 			ani.SetBool("walk",true);

		// 			float h = Input.GetAxis("Horizontal");  
		// 			float v = Input.GetAxis("Vertical");  
		// 			// Debug.Log("h:"+keytab+"  v:"+v);
		// 			ani.SetFloat("lr", h, 0.01f, Time.deltaTime);  
		// 			ani.SetFloat("fb", v, 0.01f, Time.deltaTime); 
		// 		}
		//     }
		// }else
		// {    ani.SetBool("fly",false);
		// 	 ani.SetBool("walk",false);
        //     if(is_onFloor)
		// 	{
		// 		ani.SetFloat("wait_up",0, 0.02f, Time.deltaTime);
		// 	}else{
		// 		ani.SetFloat("wait_up",1, 0.02f, Time.deltaTime);
		// 	}
		//     ani.SetFloat("lr", 0, 0.01f, Time.deltaTime);  
        //     ani.SetFloat("fb", 0, 0.01f, Time.deltaTime); 
		//     ani.SetFloat("fly_lr",0);
		// 	ani.SetFloat("fly_fb",0);
		// }
	}
	
	void setKeytab()//标记WSAD哪个键被按下和按下的时间
	{
		 if(Input.GetKey(KeyCode.W))
		{keytime=Time.fixedTime;keytab=KeyCode.W;}
		else if(Input.GetKey(KeyCode.S))
		{keytime=Time.fixedTime;keytab=KeyCode.S;}
		else if(Input.GetKey(KeyCode.A))
		{keytime=Time.fixedTime;keytab=KeyCode.A;}
		else if(Input.GetKey(KeyCode.D))
		{keytime=Time.fixedTime;keytab=KeyCode.D;}
	}
}
