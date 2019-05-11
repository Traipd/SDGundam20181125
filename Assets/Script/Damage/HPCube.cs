using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCube : MonoBehaviour {
	public HealthHP	hp;
	public float offset_up=2f;
	private Vector3 offset_doudong;
	private float max_hp;
	private Transform tra;
	public Transform parent_transform;
	private float last_health_value;
	public float delay_time=5;//扣血后HP条显示时间
	private float last_time=0;
	private bool start_counttime_flag=false;
	// Use this for initialization
	void Start () {
		tra=GetComponent<Transform>();
		// parent_transform=GetComponentInParent<Transform>();
		max_hp=hp.health_value;
		GetComponent<Renderer>().enabled=false;
		last_health_value=hp.health_value;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(GameObject.FindGameObjectWithTag("MainCamera")!=null)
		{
			tra.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform.position);	
		}
		tra.position=parent_transform.position+Vector3.up*offset_up+offset_doudong*0.1f;
		tra.localScale=new Vector3(hp.health_value/max_hp,GetComponent<Transform>().localScale.y,GetComponent<Transform>().localScale.z);

		delay_visual();
		
	}
	///<summary>扣血后HP条显示一段时间</summary>
	void delay_visual()
	{
		if(hp.health_value!=last_health_value)
		{
			last_health_value=hp.health_value;
			StartCoroutine("patch_ani_deult");
			start_counttime_flag=true;
		}

		if(start_counttime_flag)
		{
			start_counttime_flag=false;
			last_time=Time.time;
		}

		if(Time.time-last_time<delay_time)
		{
			GetComponent<Renderer>().enabled=true;
		}else
		{
			GetComponent<Renderer>().enabled=false;
		}
		
	}
	///<summary>扣血后HP条抖动</summary>
	IEnumerator patch_ani_deult()
	{
		for(float i=0;i<0.3f;i+=Time.fixedDeltaTime)
		{
			offset_doudong=new Vector3(Mathf.Sin(Time.fixedTime*64),Mathf.Sin(Time.fixedTime*64+1.57f),Mathf.Sin(Time.fixedTime*64+1.57f));
			yield return null;
		}
		offset_doudong=Vector3.zero;
		yield return null;
	}
}
