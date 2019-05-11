using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinZhanScript : MonoBehaviour {
	public AudioClip audioclip;
    public ExplodeManager explode_man;
    public int explode_number=1;
	public Transform Ftransform;
    private bool active_flag=true;
    bool FireFlag=false,StopFlag=false;

	// private Vector3 position;//防止全固定刚体碰撞导致的相对于父级坐标系位移，记录原位置信息
	// private Quaternion rotation;
	void Start () {	
        if(explode_man==null)
        {
            explode_man=GameObject.FindGameObjectWithTag("ExplodeManager").GetComponent<ExplodeManager>();
        }
        // blast_object=(GameObject)Instantiate(blast_object,Vector3.zero,Quaternion.identity);
        // if(blast_object.GetComponent<MeshRenderer>()!=null)
        // {
        //     blast_object.GetComponent<MeshRenderer>().enabled=false;
        // }      

	}
	// Update is called once per frame
	void FixedUpdate() {
	    getAttackSystemInObject();

        if(active_flag)//物理运动
        {
            GetComponent<Transform>().position=Ftransform.position;
			GetComponent<Transform>().rotation=Ftransform.rotation;
        }
		
	    setUnActive_1();//delay时间后设置子弹为不可运行状态的附属函数
        // blast_canSeeDuringTime_1();//让爆炸在显示一定时间后消失的附属函数
	}
	//必须带有Rigidboay组件才能触发
    //开始碰撞
    void OnCollisionEnter(Collision collision)
    {
        // setUnActive();
        // blast_object.GetComponent<Transform>().position=collision.contacts[0].point;
        // blast_canSeeDuringTime(0.2f);
        // StopFlag=true;
    }
	//开始接触
    void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.GetComponent<HealthHP>()!=null)
        {
            if(!collider.gameObject.GetComponent<HealthHP>().noeffect_flag)
            {
                // blast_object.GetComponent<Transform>().position=GetComponent<Transform>().position;
                // blast_canSeeDuringTime(0.2f);
                explode_man.blastI_at_V3(explode_number,GetComponent<Transform>().position);
                AudioSource.PlayClipAtPoint(audioclip,transform.position);
            }
        }
    }
    // 接触结束
    void OnTriggerExit(Collider collider) {
        if(collider.gameObject.GetComponent<HealthHP>()!=null)
        {
            if(!collider.gameObject.GetComponent<HealthHP>().noeffect_flag)
            {
                // blast_object.GetComponent<Transform>().position=GetComponent<Transform>().position;
                // blast_canSeeDuringTime(0.2f);
                explode_man.blastI_at_V3(explode_number,GetComponent<Transform>().position);
                AudioSource.PlayClipAtPoint(audioclip,transform.position);
            }
        }
    }
    // 接触持续中
    void OnTriggerStay(Collider collider) {
        if(collider.gameObject.GetComponent<HealthHP>()!=null)
        {
            if(!collider.gameObject.GetComponent<HealthHP>().noeffect_flag)
            {
                // blast_object.GetComponent<Transform>().position=GetComponent<Transform>().position;
                // blast_canSeeDuringTime(0.2f);
                explode_man.blastI_at_V3(explode_number,GetComponent<Transform>().position);
                AudioSource.PlayClipAtPoint(audioclip,transform.position);
            }
        }
       
        // Debug.Log("接触持续中");
    }

    ///<summary>从AttackSystemInObject中读取信息并做出反应</summary>
    void getAttackSystemInObject()
    {
        FireFlag=GetComponent<AttackSystemInObject>().getFireFlag();
        if(FireFlag)
        {
            active_flag=true;
            FireFlag=false;
            GetComponent<AttackSystemInObject>().setFireFlag(false);
            setActive();
        }
        StopFlag=GetComponent<AttackSystemInObject>().getStopFlag();
        if(StopFlag)
        {
            active_flag=false;
            StopFlag=false;
            GetComponent<AttackSystemInObject>().setStopFlag(false);
            setUnActive();
        }
    }

    ///<summary>设置子弹为可运行状态</summary>
    public void setActive()
    {
        active_flag=true;
        GetComponent<Collider>().enabled=true;
        if(GetComponent<MeshRenderer>()!=null)
        {
            GetComponent<MeshRenderer>().enabled=true;
        }
        if(GetComponentInChildren<MeshRenderer>()!=null)
        {
            MeshRenderer[] m=GetComponentsInChildren<MeshRenderer>();
            for(int i=0;i<m.Length;i++)
            {
                m[i].enabled=true;
            }
        }

        // if(trailingMesh2!=null)
        // {
        //     trailingMesh2.setVisual();
        // }
    
    }
    ///<summary>设置子弹为不可运行状态</summary>
    public void setUnActive()
    {
        active_flag=false;
        GetComponent<Collider>().enabled=false;
        if(GetComponent<MeshRenderer>()!=null)
        {
            GetComponent<MeshRenderer>().enabled=false;
        }
        if(GetComponentInChildren<MeshRenderer>()!=null)
        {
            MeshRenderer[] m=GetComponentsInChildren<MeshRenderer>();
            for(int i=0;i<m.Length;i++)
            {
                m[i].enabled=false;
            }
        }
        // if(trailingMesh2!=null)
        // {
        //     trailingMesh2.setUnVisual();
        // }
       
    }
    
    ///<summary>delay时间后设置子弹为不可运行状态</summary>
    public void setUnActive(float delay)
    {
       active_flag=false;
       GetComponent<Collider>().enabled=false;
       setUnActive_1_flag=true;
       setUnActive_delay=delay;
    }
    float setUnActive_delay=0,setUnActive_time=0;
    bool setUnActive_1_flag=false;
    void setUnActive_1()
    {
        if(setUnActive_1_flag)
        {
            // Debug.Log("setUnActive_time:"+setUnActive_time);
            setUnActive_time+=Time.fixedDeltaTime;
            if(setUnActive_time>setUnActive_delay)
            {
                setUnActive_time=0;setUnActive_1_flag=false;

                if(GetComponent<MeshRenderer>()!=null)
                {
                    GetComponent<MeshRenderer>().enabled=false;
                }
                if(GetComponentInChildren<MeshRenderer>()!=null)
                {
                    MeshRenderer[] m=GetComponentsInChildren<MeshRenderer>();
                    for(int i=0;i<m.Length;i++)
                    {
                        m[i].enabled=false;
                    }
                }

                // if(trailingMesh2!=null)
                // {
                //     trailingMesh2.setUnVisual();
                // }
             
            }
        }
    }

    // ///<summary>让爆炸在显示一定时间后消失</summary>  
    // void blast_canSeeDuringTime(float delay)
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
    //         if(blast_canSeeDuringTime_time>=blast_canSeeDuringTime_delay)
    //         {
    //             blast_canSeeDuringTime_time=0;blast_canSeeDuringTime_flag=false;
    //             if(blast_object.GetComponent<MeshRenderer>()!=null)
    //             {
    //                 blast_object.GetComponent<MeshRenderer>().enabled=false;
    //             }
    //         }else
    //         {
    //             if(blast_object.GetComponent<MeshRenderer>()!=null)
    //             {
    //                 blast_object.GetComponent<MeshRenderer>().enabled=true;
    //             }
    //         }
    //     }
    // }
}
