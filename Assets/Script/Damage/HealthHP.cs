using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHP : MonoBehaviour {

	public float health_value=10f;//生命值
	public int damage_resist_level=1;
    public bool normal_hurt=false;//普通受伤持续中标记(无敌中不会受伤)
	public float recover_time=0.3f;//僵直时间
	public bool recovering =false;//僵直持续中标记(无敌中不能被僵直)
	public float noeffect_time=0.05f;//僵直时的无敌时间
	public bool noeffect_flag=false;//无敌持续中的标记(无敌前必然僵直)
	public float downing_value=100;//倒地值
	public float downing_noeffect_time=2f;//倒地时间


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {	
	}
	private float normal_delay=0,recover_delay=0,noeffect_delay=0;//,down_delay=0
	
	void FixedUpdate()
	{
		reDown_value();
		if(normal_hurt)//普通受伤
		{
			if(normal_delay>=0.2f)
			{
				normal_hurt=false;
				normal_delay=0;
			}
			normal_delay+=Time.fixedDeltaTime;
		}
		if(recovering)//僵直
		{
			if(recover_delay>=recover_time)
			{
				recovering=false;
				recover_delay=0;
			}
			recover_delay+=Time.fixedDeltaTime;
		}
		if(noeffect_flag)//无敌
		{
			if(downing_value<=0)//倒地
			{
				// if(down_delay>=downing_noeffect_time)
				// {
				// 	noeffect_flag=false;
				// 	down_delay=0;
				// 	downing_value=100;
				// }
				// noeffect_delay+=Time.fixedDeltaTime;
			}
			else//非倒地的情况
			{
				if(noeffect_delay>=noeffect_time)
				{
					noeffect_flag=false;
					noeffect_delay=0;
				}
				noeffect_delay+=Time.fixedDeltaTime;
			}	
		}

	}
	private float nothing_time=0;
	///<summary>倒地值恢复机制</summary>
	void reDown_value()
	{
		if(!recovering)
		{
           nothing_time+=Time.fixedDeltaTime;
		}else
		{
			nothing_time=0;
		}
		if(nothing_time>=downing_noeffect_time)
		{
			downing_value=100;
		}
	}
	///<summary>被攻击时使用的函数</summary>
	public void onDamage(float damage_value,int damage_level,float damage_down)
	{
		if(!noeffect_flag)
		{
			health_value-=damage_value;
			downing_value-=damage_down;
			if(damage_level>=damage_resist_level)
			{
				recovering=true;
				noeffect_flag=true;
			}else
			{
				normal_hurt=true;
			}
		}
	}

}
