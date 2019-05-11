using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

	public AudioClip audioclip_A;
	public AudioClip audioclip_Boss;
	public GameObject win_text; 
	public GameObject[] gobject_ai_all;
	private ExplodeManager explodeM;
	// Use this for initialization
	void Start () {
		if(explodeM==null)
		{
			explodeM=GameObject.FindGameObjectWithTag("ExplodeManager").GetComponent<ExplodeManager>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0;i<gobject_ai_all.Length;i++)
		{
			if(gobject_ai_all[i].GetComponent<HealthHP>()!=null)
			{
				if(gobject_ai_all[i].GetComponent<HealthHP>().health_value<=0)
				{
					if(gobject_ai_all[i].activeSelf)
					{
						explodeM.blastI_at_V3(0,gobject_ai_all[i].transform.position+Vector3.up*1f);
						if(audioclip_A!=null)
						{
							AudioSource.PlayClipAtPoint(audioclip_A,transform.position);
						}
					}
					gobject_ai_all[i].SetActive(false);
				}
			}
		}
		call_boss();//唤醒boss,进入boss阶段
		win_UI();//击杀所有敌人后UI提示
	}
	
	bool call_boss_canflag=true;
	void call_boss()
	{
		if(!gobject_ai_all[gobject_ai_all.Length-2].activeSelf&&call_boss_canflag)
		{
			if(audioclip_Boss!=null)
			{
				AudioSource.PlayClipAtPoint(audioclip_Boss,GameObject.FindGameObjectWithTag("Player").transform.position);
			}
			gobject_ai_all[gobject_ai_all.Length-1].SetActive(true);
			call_boss_canflag=false;
		}
	}

	bool has_win_flag=false;
	void win_UI()
	{
		if(!has_win_flag)
		{
			bool has_AI=false;
			for(int i=0;i<gobject_ai_all.Length;i++)
			{
				if(gobject_ai_all[i].activeSelf)
				{
					has_AI=true;
				}
			}
			if(!has_AI)
			{
				win_text.SetActive(true);
				StartCoroutine(win_text_SetActive_false());
				has_win_flag=true;
			}
		}
	}
	IEnumerator  win_text_SetActive_false()
	{
		float t=0;
		while(t<5f)
		{
			t+=Time.deltaTime;
			yield return null;
		}
		win_text.SetActive(false);
		yield return null;
	}
}
