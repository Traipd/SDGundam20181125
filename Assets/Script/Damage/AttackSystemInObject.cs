using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystemInObject : MonoBehaviour {

	bool fire_flag=false;
	bool is_fired_flag=false;//是否正在运行flag
	bool stop_flag=false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	///<summary>设置是否开火</summary>
	public void setFireFlag(bool f)
	{
		// Debug.Log(is_fired_flag+" "+fire_flag+" "+f);
		if(!is_fired_flag&&f)
		{
			fire_flag=f;
			is_fired_flag=true;
		}
		if(!f)
		{
			fire_flag=f;
		}
	}
	///<summary>读取是否开火</summary>
	public bool getFireFlag()
	{
		return fire_flag;
	}
	///<summary>设置是否停火</summary>
	public void setStopFlag(bool f)
	{
		if(is_fired_flag&&f)
		{
			stop_flag=f;
			is_fired_flag=false;
		}
		if(!f)
		{
			stop_flag=f;
		}	
		
	}
	///<summary>读取是否停火</summary>
	public bool getStopFlag()
	{
		return stop_flag;
	}
}
