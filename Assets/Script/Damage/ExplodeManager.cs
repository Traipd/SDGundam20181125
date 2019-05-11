using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeManager : MonoBehaviour {

    [System.Serializable]
    public struct Blast_object_list{
		public GameObject blast_object;
	    public float blast_object_delay;//爆炸物体显示时间
	}

	public Blast_object_list[] blast_object_list;//爆炸物体主要信息列表
	private GameObjectList[] blast_object_stop;//已停止工作的爆炸物体
	private GameObjectList[] blast_object_active;//正在工作的爆炸物体
	private FloatList[] blast_object_activetime;//正在工作的爆炸物体的计时
    private int blast_object_number;//使用哪个爆炸物体
    private Vector3 blast_position;//爆炸位置

    bool blast_flag=false;

	// Use this for initialization
	void Start () {//初始化
        blast_object_stop=new GameObjectList[blast_object_list.Length];
        blast_object_active=new GameObjectList[blast_object_list.Length];
        blast_object_activetime=new FloatList[blast_object_list.Length];
		for(int i=0;i<blast_object_list.Length;i++)
        {
            blast_object_stop[i]=new GameObjectList();
            blast_object_active[i]=new GameObjectList();
            blast_object_activetime[i]=new FloatList();
        }
        // blast_flag=true;
        // blast_object_number=0;
        // blast_position=Vector3.up;
	}
	
	// Update is called once per frame
	void Update () {
		if(blast_flag)
        {
            blast_flag=false;
            GameObject g;
            if(blast_object_stop[blast_object_number].Length<=0)//停止工作的队列是否有剩余
            {
                g=(GameObject)Instantiate(blast_object_list[blast_object_number].blast_object,blast_position,Quaternion.identity);
            }else
            {
                g=blast_object_stop[blast_object_number].putOut();
                g.GetComponent<Transform>().position=blast_position;
            }
                blast_object_activetime[blast_object_number].setIn(0f);
                blast_object_active[blast_object_number].setIn(g);
                g.GetComponent<Renderer>().enabled=true;
        }

        blast__Recover();//爆炸物体按时回收
	}

    ///<summary>爆炸物体按时回收</summary>
    void blast__Recover()
    {
        for(int i=0;i<blast_object_active.Length;i++)//每种
        {
            for(int j=0;j<blast_object_active[i].Length;j++)//工作中每个
            {
                if(blast_object_activetime[i].readI(j)>blast_object_list[i].blast_object_delay)//时间对比
                {
                    GameObject g=blast_object_active[i].putOutI(j);
                    g.GetComponent<Renderer>().enabled=false;
                    blast_object_stop[i].setIn(g);//工作转停止
                }else
                {
                    blast_object_activetime[i].writeI(j,blast_object_activetime[i].readI(j)+Time.deltaTime);//计时增长
                }
            }
        }
    }

    ///<summary>让某种爆炸在Vector3位置发生</summary>
    public void blastI_at_V3(int i,Vector3 v)
    {
        blast_flag=true;
        blast_object_number=i;
        blast_position=v;
    }

	// ///<summary>让爆炸在显示一定时间后消失</summary>  
    // public void blast_canSeeDuringTime(float delay)
    // {
    //     blast_canSeeDuringTime_flag=true;blast_canSeeDuringTime_delay=delay;
    // }
    // float blast_canSeeDuringTime_time=0,blast_canSeeDuringTime_delay=0;
    // bool blast_canSeeDuringTime_flag=false;
    // void blast_canSeeDuringTime_1()
    // {
    //     if(blast_canSeeDuringTime_flag)
    //     {
    //         blast_canSeeDuringTime_time+=Time.fixedDeltaTime;
    //         // if(blast_canSeeDuringTime_time>=blast_canSeeDuringTime_delay)
    //         // {
    //         //     blast_canSeeDuringTime_time=0;blast_canSeeDuringTime_flag=false;
    //         //     if(blast_object.GetComponent<MeshRenderer>()!=null)
    //         //     {
    //         //         blast_object.GetComponent<MeshRenderer>().enabled=false;
    //         //     }
    //         // }else
    //         // {
    //         //     if(blast_object.GetComponent<MeshRenderer>()!=null)
    //         //     {
    //         //         blast_object.GetComponent<MeshRenderer>().enabled=true;
    //         //     }
    //         // }
    //     }
    // }
}
