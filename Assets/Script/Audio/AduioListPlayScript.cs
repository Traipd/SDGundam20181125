using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AduioListPlayScript : MonoBehaviour {
	[Tooltip("将音频与对象的顺序位置对应起来便于统一调用")]
	public AudioClip[] audioclip_array_ation;
	private bool[] audioclip_array_ation_doneflag;
	public AudioClip[] audioclip_array_attack;
	// private bool[] audioclip_array_attack_doneflag;
	AudioSource audiosoure_loop;
	public bool play_flag=false;
	public int clip_number=0;
	// Use this for initialization
	void Start () {
		audiosoure_loop=GetComponent<AudioSource>();
		audioclip_array_ation_doneflag=new bool[audioclip_array_ation.Length];
		// audioclip_array_attack_doneflag=new bool[audioclip_array_attack.Length];
	}
	
	// Update is called once per frame
	void Update () {
		if(play_flag)
		{
			if(!audiosoure_loop.isPlaying)
			{
				audiosoure_loop.clip=audioclip_array_ation[clip_number];
				audiosoure_loop.loop=true;
				audiosoure_loop.Play();
			}
		}else
		{
			if(audiosoure_loop.isPlaying)
			{
				audiosoure_loop.Stop();
			}
		}
		
	}
	///<summary>播放一个动作音频并禁止重复播放，解除另一个音频的禁止</summary>
	public void play_ation_audio_once(int i,int j,Vector3 position)
	{
		if(!audioclip_array_ation_doneflag[i])
		{
			AudioSource.PlayClipAtPoint(audioclip_array_ation[i],position);
			audioclip_array_ation_doneflag[i]=true;
			audioclip_array_ation_doneflag[j]=false;
		}
	}

	///<summary>播放一个动作音频</summary>
	public void play_ation_audio(int i,Vector3 position)
	{
		AudioSource.PlayClipAtPoint(audioclip_array_ation[i],position) ;
	}
	///<summary>播放一个攻击音频</summary>
	public void play_attack_audio(int i,Vector3 position)
	{
		AudioSource.PlayClipAtPoint(audioclip_array_attack[i],position) ;
	}
}
