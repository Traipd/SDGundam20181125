using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WuMingYiDuanCollider4 : MonoBehaviour {

	public HealthHP hP_script;
	public wuqilanScript wuqilan;
	public AttackSystem attackSystem;//请避免频繁set，导致栈拥挤，物体创建与实例反应慢
	public AduioListPlayScript aduioplayer;
	[Tooltip("每个脚步声之间的水平距离")]
	public float stepAduio_distance=1;
	// public is_Onfloor is_Onfloorscript;
	public TrailingMesh2 trailingmesh_1;
	public TrailingMesh2 trailingmesh_2;
	public Animator ani;
    public Transform camera_transform;
	public float g_inScene=5;//重力参数
	public float speed=3;//速度参数
	public Slider slider;
	public float fly_slider_reflex=1;//飞翔条回复参数
	public float fly_slider_cost=1;//飞翔条消耗参数
	public GameObject[] fly_fire;//飞翔火焰模型
	public GameObject[] jinzhan1_mod;//近战1号武器模型
    private int wuqi_number=1;
	
	//是否浮空//是否在地面//是否在飞翔//是否能起飞
	bool is_fu=false,is_onFloor=false,is_fly=false,is_canfly=true;
	//关于按键的记录
	float w_keytime=0,s_keytime=0,a_keytime=0,d_keytime=0,space_keytime=0,mouse0_keytime=0,mouse1_keytime=0,mouse2_keytime=0;
	bool w_downing=false,s_downing=false,a_downing=false,d_downing=false,space_downing=false,mouse0_downing=false,mouse1_downing=false,mouse2_downing=false;
	bool w_2downing=false,s_2downing=false,a_2downing=false,d_2downing=false;
	///<summary>关于完全互斥的动画的记录</summary>
	string[][] action_ani_name={new string[]{"first_1","first_2","first_3"},//,"first_4","first_5"
								new string[]{"second_1"},
								new string[]{"third_1","third_2"},
								new string[]{"hurt"}};//受伤动画请放在最后一个							
	float[][] action_ani_doingtime={new float[]{0.4f,0.4f,0.4f},//,0.4f,0.4f
									new float[]{1f},
									new float[]{0.5f,0.5f},
									new float[]{0.5f}};//一个动作的执行时间
	float[][] action_ani_delaytime={new float[]{0.8f,0.8f,0.45f},//,0.8f,0.5f
									new float[]{1.1f},
									new float[]{0.85f,0.55f},
									new float[]{0.5f}};//过了这个时间后才会退出动作（与action_ani_doingtime时间之差为可连击时间）
	int action_ani_arraynumber=0;
	int action_ani_number=0;
	float action_ani_lasttime=0;
	bool action_ani_doing=false;
	///<summary>关于叠加混合的动画的记录</summary>
	string[] additional_ani_name={"add_1","add_hurt"};//受伤动画请放在最后一个,(此受伤动画仅受HealthHP中的时间影响)
	float[] additional_ani_doingtime={0.4f,0.15f};//过了这个时间后才会退出动作
	float[] additional_ani_delaytime={2f,0.05f};//过了这个时间后才能再进行动作（与additional_ani_doingtime时间之差为攻击时间间隔）
	int additional_ani_number=0;
	float additional_ani_lasttime=0;
	bool additional_ani_doing=false;
	bool stop_flag=false;

	// Use this for initialization
	void Start () {
		g_inScene=-g_inScene;
	}
	//必须带有Rigidboay组件才能触发
	 // 接触结束
    void OnTriggerExit(Collider collider) {
		if(!collider.tag.Equals("Attack"))
		{
			is_onFloor=false;
			StartCoroutine("OnTriggerExitr_CD");
		}   
    }

    // 接触持续中
    void OnTriggerStay(Collider collider) {
		if(!collider.tag.Equals("Attack"))
		{
			if(!OnTriggerExit_CD_flag)
			{
				is_onFloor=true;
			}
		}
		
    }
	bool OnTriggerExit_CD_flag=false;
	IEnumerator OnTriggerExitr_CD()
	{
		OnTriggerExit_CD_flag=true;
		float t=0;
		while(t<0.2f)
		{
			t+=Time.fixedDeltaTime;
			yield return null;
		}
		OnTriggerExit_CD_flag=false;
		yield return null;
	}
	
	void OnTriggerEnter(Collider collider){
		if(!collider.tag.Equals("Attack"))
		{
			if(!OnTriggerEnter_CD_flag)
			{
				aduioplayer.play_ation_audio(3,transform.position);
				StartCoroutine("OnTriggerEnter_CD");
			}
		}
	}
	bool OnTriggerEnter_CD_flag=false;
	IEnumerator OnTriggerEnter_CD()
	{
		OnTriggerEnter_CD_flag=true;
		float t=0;
		while(t<0.5f)
		{
			t+=Time.fixedDeltaTime;
			yield return null;
		}
		OnTriggerEnter_CD_flag=false;
		yield return null;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			stop_flag=!stop_flag;
		}
		if(!stop_flag)
		{
			transform.eulerAngles=new Vector3(0,camera_transform.eulerAngles.y,0);
		}
		
	}

	float h ,v;  

	void FixedUpdate(){
		h = Input.GetAxis("Horizontal");  
		v = Input.GetAxis("Vertical"); 
		if(v==0){v=0.1f;}
		
		checkTakeKey();//按键检测合集
		patch_1();//全局的动画defult条件设为false以及防动画未能及时跳出

		huchi_action();//互斥动作的处理(内含优先级较低的混合动作)
		audio_Play();//普通动作的音效触发

		//必然事件
		if(is_onFloor&&!is_fly&&!is_fu)
		{
			slider.value=Mathf.Min(slider.value+0.3f*fly_slider_reflex*Time.fixedDeltaTime,1);//飞翔条回复
		}
		if(!is_onFloor)
		{
			transform.Translate(Vector3.up*g_inScene*Time.fixedDeltaTime);
		}else
		{
			transform.Translate(Vector3.up*g_inScene*Time.fixedDeltaTime*0.1f);
		}
		

	}

	float hurt_up_delay=0;
	///<summary>互斥动作的处理</summary>
	void huchi_action()
	{
		if(hP_script.downing_value<=0)//受伤倒地
		{
			
			hurt_up_delay+=Time.fixedDeltaTime;
			attackSystem.attackObject_Recover_one(2);
			if(hurt_up_delay>=hP_script.downing_noeffect_time-0.18f)
			{
				ani.SetBool("hurt_down",false);
				ani.SetBool("hurt_up",true);
				aduioplayer.play_ation_audio_once(5,4,transform.position);			
			}else{
				ani.SetLayerWeight(3,1);
				ani.SetBool("hurt_down",true);
				aduioplayer.play_ation_audio_once(4,5,transform.position);
			}

			if(action_ani_number!=0)//重置动作
			{
				ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number-1],false);
				action_ani_number=0;		
				trailingmesh_1.setUnVisual();
				trailingmesh_2.setUnVisual();
			}action_ani_doing=false;
			additional_ani_doing=false;
			ani.SetBool(additional_ani_name[additional_ani_number],false);
			ani.SetLayerWeight(1,0);
			ani.SetBool("defult",true);
			setWASDandSpaceDefult();//将WASD和空格相关的动画设置为默认状态
			
		}else
		{	
			
			hurt_up_delay=0;
			ani.SetLayerWeight(3,0);
			ani.SetBool("hurt_up",false);
			if(hP_script.recovering)//如果出现受伤僵直
			{
				ani.SetLayerWeight(3,Mathf.Lerp(ani.GetLayerWeight(3),0.8f,0.8f));
				ani.SetBool(action_ani_name[action_ani_name.Length-1][0],true);

				attackSystem.attackObject_Recover_one(2);
				if(action_ani_number!=0)//重置动作
				{	
					ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number-1],false);
					action_ani_number=0;	
					trailingmesh_1.setUnVisual();
					trailingmesh_2.setUnVisual();
				}action_ani_doing=false;
				additional_ani_doing=false;
				ani.SetBool(additional_ani_name[additional_ani_number],false);
				ani.SetLayerWeight(1,0);
				ani.SetBool("defult",true);
				setWASDandSpaceDefult();//将WASD和空格相关的动画设置为默认状态
			}
			else
			{
				ani.SetLayerWeight(3,Mathf.Lerp(ani.GetLayerWeight(3),0,0.8f));
				ani.SetBool(action_ani_name[action_ani_name.Length-1][0],false);

				huchi_action_gongji();//互斥动作中攻击部分的处理(含WASD和空格)
				hunhe_action();//混合动作的处理	
			}
		}

        
	}

	///<summary>互斥动作中攻击部分的处理</summary>
	void huchi_action_gongji()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))//按下鼠标左键
		{		
			if(wuqi_number==1)//一号武器
			{
				if(action_ani_number<action_ani_name[action_ani_arraynumber].Length)
				{
					if(mouse0_keytime-action_ani_lasttime>action_ani_doingtime[action_ani_arraynumber][max(action_ani_number-1,0)]||action_ani_number==0)
					{
						action_ani_lasttime=Time.fixedTime;
						if(action_ani_number-1>=0)
						{
							ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number-1],false);
						}
						ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number],true);
						action_ani_number++;
						action_ani_doing=true;
						trailingmesh_1.setVisual();
						trailingmesh_2.setVisual();
					}
				}
			}
			if(wuqi_number==3)//三号武器
			{
				if(action_ani_number<action_ani_name[action_ani_arraynumber].Length)
				{
					if(mouse0_keytime-action_ani_lasttime>action_ani_doingtime[action_ani_arraynumber][max(action_ani_number-1,0)]||action_ani_number==0)
					{
						action_ani_lasttime=Time.fixedTime;
						if(action_ani_number-1>=0)
						{
							ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number-1],false);
						}
						ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number],true);
						action_ani_number++;
						action_ani_doing=true;
						// trailingmesh_1.setVisual();
						// trailingmesh_2.setVisual();
					}
				}
			}
			if(wuqi_number==4)//四号武器
			{
				if(action_ani_number<action_ani_name[action_ani_arraynumber].Length)
				{
					if(mouse0_keytime-action_ani_lasttime>action_ani_doingtime[action_ani_arraynumber][max(action_ani_number-1,0)]||action_ani_number==0)
					{
						action_ani_lasttime=Time.fixedTime;
						if(action_ani_number-1>=0)
						{
							ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number-1],false);
							attackSystem.set_AttackObject_number(4);
						}else
						{
							attackSystem.set_AttackObject_number(3);
						}
						ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number],true);
						action_ani_number++;
						action_ani_doing=true;
					}
				}
			}
		}

		if(action_ani_doing)//当互斥动画进行时
		{
			if(Time.fixedTime-action_ani_lasttime<action_ani_doingtime[action_ani_arraynumber][action_ani_number-1])
			{
				if(wuqi_number==1)//一号武器的动作效果
				{
					transform.Translate(Vector3.forward*speed*Time.deltaTime);
				}
				if(wuqi_number==3)//三号武器的动作效果
				{
					// transform.Translate(Vector3.forward*speed*2.5f*Time.deltaTime);
					if(!is_onFloor)
					{
						transform.Translate(Vector3.up*-g_inScene*Time.deltaTime);
					}
				}
				if(wuqi_number==4)//四号武器的动作效果
				{
					if(action_ani_number==1)
					{
						transform.Translate(Vector3.forward*speed*2f*Time.deltaTime);
					}   transform.Translate(Vector3.forward*speed*0.5f*Time.deltaTime);	

					
				}
			}

			if(wuqi_number==1)//一号武器的攻击效果
			{
				if(Time.fixedTime-action_ani_lasttime>=action_ani_doingtime[action_ani_arraynumber][action_ani_number-1]&&Time.fixedTime-action_ani_lasttime<=action_ani_doingtime[action_ani_arraynumber][action_ani_number-1]+0.01f)
				{
					attackSystem.set_Attack_flag(true);//创建或现实攻击用物体
				}	
			}
			if(wuqi_number==3)//三号武器的攻击效果
			{
				if(Time.fixedTime-action_ani_lasttime>=action_ani_doingtime[action_ani_arraynumber][action_ani_number-1]-0.4f&&Time.fixedTime-action_ani_lasttime<=action_ani_doingtime[action_ani_arraynumber][action_ani_number-1]-0.39f)
				{
					attackSystem.set_Attack_flag(true);//创建或现实攻击用物体
				}
			}
			if(wuqi_number==4)//四号武器的攻击效果
			{
				if(Time.fixedTime-action_ani_lasttime>=action_ani_doingtime[action_ani_arraynumber][action_ani_number-1]&&Time.fixedTime-action_ani_lasttime<=action_ani_doingtime[action_ani_arraynumber][action_ani_number-1]+0.01f)
				{
					attackSystem.set_Attack_flag(true);//创建或现实攻击用物体
				}	
			}
			

			if(Time.fixedTime-action_ani_lasttime>action_ani_delaytime[action_ani_arraynumber][action_ani_number-1])//倒计时
			{		
				ani.SetBool(action_ani_name[action_ani_arraynumber][action_ani_number-1],false);
				ani.SetBool("defult",true);
				action_ani_number=0;
				action_ani_doing=false;
				trailingmesh_1.setUnVisual();
				trailingmesh_2.setUnVisual();
			}	
			
			setWASDandSpaceDefult();//将WASD和空格相关的动画设置为默认状态
		}
		else
		{
			WASD_and_Space_Down();//按下WASD和空格键时的反应
		}
	}

	///<summary>混合动作的处理</summary>
	void hunhe_action()
	{
		if(mouse0_downing)//按住鼠标右键
		{
			if(wuqi_number==2)//二号武器
			{
				if(Time.fixedTime-additional_ani_lasttime>additional_ani_delaytime[additional_ani_number])
				{
					additional_ani_lasttime=Time.fixedTime;
					ani.SetLayerWeight(1,1);
					ani.SetBool(additional_ani_name[additional_ani_number],true);
					additional_ani_doing=true;
				}
			}				
		}

		if(additional_ani_doing)//当叠加混合的动画进行时
		{	
			if(Time.fixedTime-additional_ani_lasttime>additional_ani_doingtime[additional_ani_number])//倒计时
			{
				attackSystem.set_Attack_flag(true);//创建或现实攻击用物体
				ani.SetLayerWeight(1,0);
				ani.SetBool(additional_ani_name[additional_ani_number],false);
				additional_ani_doing=false;
			}
		}
		else{
			if(hP_script.normal_hurt)//普通受伤
			{
				ani.SetLayerWeight(2,1);
				ani.SetBool(additional_ani_name[additional_ani_name.Length-1],true);
			}else
			{
				ani.SetLayerWeight(2,0);
				ani.SetBool(additional_ani_name[additional_ani_name.Length-1],false);
			}
		} 
	}

	///<summary>按下WASD和空格键时的反应</summary>
	void WASD_and_Space_Down(){
		//WSAD
        if(w_downing==false&&s_downing==false&&a_downing==false&&d_downing==false)//无WSAD
		{
			setWASDandSpaceDefult();//将WASD和空格相关的动画设置为默认状态
		}
		
		if(w_2downing||s_2downing||a_2downing||d_2downing||is_fly)//WSAD连击
		{
			transform.Translate(new Vector3(3*h,0,3*v)*speed*Time.deltaTime);//位移法
			if(!is_onFloor)
			{
				transform.Translate(Vector3.up*0.5f*-g_inScene*Time.deltaTime);
			}
			
			slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);

			ani.SetBool("walk",false);
			ani.SetBool("fly",true);
			ani.SetFloat("fly_lr", h);  
			ani.SetFloat("fly_fb", v);

			is_fly=true;
			if(slider.value==0)
			{
               ani.SetBool("fly",false);

			   is_fly=false;
			}

			setFlyfireVisible();
			
		}
		else if(w_downing==true||s_downing==true||a_downing==true||d_downing==true)//WSAD单击
		{                 
			transform.Translate(new Vector3(h,0,v)*speed*Time.fixedDeltaTime);//位移法

			if(is_onFloor==true)
			{
				ani.SetBool("walk",true);
				ani.SetBool("fly",false);
				ani.SetFloat("lr", h);//, 0.01f, Time.deltaTime 
				ani.SetFloat("fb", v);//, 0.01f, Time.deltaTime	

				setFlyfireUnVisible();
			}else
			{
				ani.SetBool("walk",false);
				ani.SetBool("fly",false);
				ani.SetFloat("wait_up",1);

				setFlyfireVisible();   
			}   

		}
		//空格
		if(space_downing==true)//空格单击
		{
			if(slider.value!=0&&is_canfly)
			{
				transform.Translate(Vector3.up*2*-g_inScene*Time.deltaTime);
				slider.value=Mathf.Max(slider.value-0.15f*fly_slider_cost*Time.fixedDeltaTime,0);
			}
		}
	}
	///<summary>将WASD和空格相关的动画设置为默认状态</summary>
	void setWASDandSpaceDefult(){
		is_fly=false;

		ani.SetBool("walk",false);
		ani.SetBool("fly",false);
		if(is_onFloor==true)
		{
			ani.SetFloat("wait_up",0);
			setFlyfireUnVisible();
		}else{
			ani.SetFloat("wait_up",1);
			setFlyfireVisible();   
		}   
	}
	Vector3 audio_last_position;
	///<summary>普通动作的音效触发</summary>
	void audio_Play()
	{
		if(Mathf.Pow(transform.position.x-audio_last_position.x,2)
		+Mathf.Pow(transform.position.z-audio_last_position.z,2)>Mathf.Pow(stepAduio_distance,2))//脚步音效
		{
			if(is_onFloor&&!is_fly)
			{
				audio_last_position=transform.position;
				aduioplayer.play_ation_audio(0,transform.position);
			}	
		}
		if(!is_onFloor||is_fly)//持续飞行音效
		{aduioplayer.play_flag=true;
		}else
		{aduioplayer.play_flag=false;
		}
	}

	///<summary>喷射音效触发</summary>
	void audio_Play_flystart()
	{
		if(!hP_script.recovering&&!(hP_script.downing_value<=0)&&slider.value>0)
		{
			aduioplayer.play_ation_audio(1,transform.position);
		}
	}

    ///<summary>按键检测合集</summary>
	void checkTakeKey(){
		checkKeyTime();
		setWuqi_number();
		setKeyState();
		setKeyTime();
	}
	///<summary>记录选择的武器号码,设置选择某一武器时应有的UI</summary>
	void setWuqi_number(){
		if(!additional_ani_doing&&!action_ani_doing)
		{
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{ wuqi_number=1;}
			if(Input.GetKeyDown(KeyCode.Alpha2))
			{ wuqi_number=2;}
			if(Input.GetKeyDown(KeyCode.Alpha3))
			{ wuqi_number=3;}
			if(Input.GetKeyDown(KeyCode.Alpha4))
			{ wuqi_number=4;}

			if(Input.GetAxis("Mouse ScrollWheel")<0)
			{
				if(wuqi_number<4){wuqi_number++;}
			}
			if(Input.GetAxis("Mouse ScrollWheel")>0)
			{
				if(wuqi_number>0){wuqi_number--;}
			}

			if(wuqi_number==1)
			{
				action_ani_arraynumber=0;wuqilan.setOnesUp(wuqi_number-1);attackSystem.set_AttackObject_number(0);
				setJinZhan1Visible();
			}else{
				setJinZhan1UnVisible();
			}

			if(wuqi_number==2)
			{
				wuqilan.setOnesUp(wuqi_number-1);attackSystem.set_AttackObject_number(1);
			}
			if(wuqi_number==3)
			{
				action_ani_arraynumber=1;wuqilan.setOnesUp(wuqi_number-1);attackSystem.set_AttackObject_number(2);
			}
			if(wuqi_number==4)
			{
				action_ani_arraynumber=2;wuqilan.setOnesUp(wuqi_number-1);attackSystem.set_AttackObject_number(3);
			}
		}	
	}

	///<summary>检测按键是否是连击两次</summary>
	void checkKeyTime(){
		
		float w=0,s=0,a=0,d=0;
		if(Input.GetKeyDown(KeyCode.W))
		{w=Time.fixedTime-w_keytime;  }
		if(Input.GetKeyDown(KeyCode.S))
		{s=Time.fixedTime-s_keytime;}
		if(Input.GetKeyDown(KeyCode.A))
		{a=Time.fixedTime-a_keytime;}
		if(Input.GetKeyDown(KeyCode.D))
		{d=Time.fixedTime-d_keytime;}

        if((w>0.018f&&w<0.25f)){w_2downing=true; audio_Play_flystart();}
		else{w_2downing=false;}
		if((s>0.018f&&s<0.25f)){s_2downing=true; audio_Play_flystart();}
		else{s_2downing=false;}
		if((a>0.018f&&a<0.25f)){a_2downing=true; audio_Play_flystart();}
		else{a_2downing=false;}
		if((d>0.018f&&d<0.25f)){d_2downing=true; audio_Play_flystart();}
		else{d_2downing=false;}
		
	}

    ///<summary>记录按键是按下或弹起状态</summary>
	void setKeyState(){
		if(Input.GetKeyDown(KeyCode.W))
		{ w_downing=true;}
		if(Input.GetKeyDown(KeyCode.S))
		{ s_downing=true;}
		if(Input.GetKeyDown(KeyCode.A))
		{ a_downing=true;}
		if(Input.GetKeyDown(KeyCode.D))
		{ d_downing=true;}
		if(Input.GetKeyDown(KeyCode.Space))
		{ space_downing=true; audio_Play_flystart();}
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{mouse0_downing=true;}
		if(Input.GetKeyDown(KeyCode.Mouse1))
		{mouse1_downing=true;}
		if(Input.GetKeyDown(KeyCode.Mouse2))
		{mouse2_downing=true;}

       
		if(!Input.GetKey(KeyCode.W))
		{ w_downing=false;}
		if(!Input.GetKey(KeyCode.S))
		{s_downing=false;}
		if(!Input.GetKey(KeyCode.A))
		{ a_downing=false;}
		if(!Input.GetKey(KeyCode.D))
		{ d_downing=false;}
		if(!Input.GetKey(KeyCode.Space))
		{ space_downing=false;}
		if(!Input.GetKey(KeyCode.Mouse0))
		{mouse0_downing=false;}
		if(!Input.GetKey(KeyCode.Mouse1))
		{mouse1_downing=false;}
		if(!Input.GetKey(KeyCode.Mouse2))
		{mouse2_downing=false;}

	}

	///<summary>记录按键按下时间</summary>
	void setKeyTime(){
		if(Input.GetKeyDown(KeyCode.W))
		{ w_keytime=Time.fixedTime;}
		if(Input.GetKeyDown(KeyCode.S))
		{ s_keytime=Time.fixedTime;}
		if(Input.GetKeyDown(KeyCode.A))
		{ a_keytime=Time.fixedTime;}
		if(Input.GetKeyDown(KeyCode.D))
		{ d_keytime=Time.fixedTime;}
		if(Input.GetKeyDown(KeyCode.Space))
		{ space_keytime=Time.fixedTime;}
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{ mouse0_keytime=Time.fixedTime;}
		if(Input.GetKeyDown(KeyCode.Mouse1))
		{ mouse1_keytime=Time.fixedTime;}
		if(Input.GetKeyDown(KeyCode.Mouse2))
		{ mouse2_keytime=Time.fixedTime;}
	}
	///<summary>令飞翔火焰可见</summary>
    void setFlyfireVisible()
	{
		for(int i=0;i<fly_fire.Length;i++)
		{
			fly_fire[i].GetComponent<SkinnedMeshRenderer>().enabled=true;
		}
	}
	///<summary>令飞翔火焰不可见</summary>
	void setFlyfireUnVisible()
	{
		for(int i=0;i<fly_fire.Length;i++)
		{
			fly_fire[i].GetComponent<SkinnedMeshRenderer>().enabled=false;
		}
	}
	///<summary>令近战武器1号可见</summary>
    void setJinZhan1Visible()
	{
		for(int i=0;i<jinzhan1_mod.Length;i++)
		{
			jinzhan1_mod[i].GetComponent<SkinnedMeshRenderer>().enabled=true;
		}
	}
	///<summary>令近战武器1号不可见</summary>
	void setJinZhan1UnVisible()
	{
		for(int i=0;i<jinzhan1_mod.Length;i++)
		{
			jinzhan1_mod[i].GetComponent<SkinnedMeshRenderer>().enabled=false;
		}
	}
	///<summary>返回两个数中的最大值</summary>
	int max(int a,int b)
	{
		if(a>=b){return a;}
		else{return b;}
	}
	float max(float a,float b)
	{
		if(a>=b){return a;}
		else{return b;}
	}
	///<summary>返回两个数中的最小值</summary>
	int min(int a,int b)
	{
		if(a<=b){return a;}
		else{return b;}
	}
	float min(float a,float b)
	{
		if(a<=b){return a;}
		else{return b;}
	}
	bool coroutine_running_flag=false;
	///<summary>全局的动画defult条件设为false以及防动画未能及时跳出</summary>;	
	void patch_1()
	{
        if(ani.GetBool("defult")&&!coroutine_running_flag)
		{
			StartCoroutine(patch_ani_deult());
			coroutine_running_flag=true;
		}
	}
	IEnumerator patch_ani_deult()
	{
		for(float i=0;i<0.01f;i+=Time.fixedTime)
		{
			yield return null;
		}
		coroutine_running_flag=false;
		ani.SetBool("defult",false);
		yield return null;
	}
}
