using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XuanZhuanFeiFuScript : MonoBehaviour {
    public AudioClip audioclip;
 	// public GameObject blast_object;
    public ExplodeManager explode_man;
    public TrailingMesh2 trailingMesh2;
	public float speed=1;
    public Vector3 direction=new Vector3(1,0,0);
    private bool active_flag=true;
    bool FireFlag=false,StopFlag=false;

    void Awake()
    {
        if(explode_man==null)
        {
            explode_man=GameObject.FindGameObjectWithTag("ExplodeManager").GetComponent<ExplodeManager>();
        }
    }
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
            transform.Translate(direction*speed*Time.deltaTime);
        }
		
	    setUnActive_1();//delay时间后设置子弹为不可运行状态的附属函数
        // blast_canSeeDuringTime_1();//让爆炸在显示一定时间后消失的附属函数
	}
	//必须带有Rigidboay组件才能触发
    //开始碰撞
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthHP>()!=null)
        {
            if(!collision.gameObject.GetComponent<HealthHP>().noeffect_flag)
            {
                setUnActive(0.15f);
                // blast_object.GetComponent<Transform>().position=collision.contacts[0].point;
                // blast_canSeeDuringTime(0.2f);
                explode_man.blastI_at_V3(1,collision.contacts[0].point);
                AudioSource.PlayClipAtPoint(audioclip,transform.position);
            }
        }else
        {
            setUnActive(0.15f);
            // blast_object.GetComponent<Transform>().position=collision.contacts[0].point;
            // blast_canSeeDuringTime(0.2f);
            explode_man.blastI_at_V3(1,collision.contacts[0].point);
            AudioSource.PlayClipAtPoint(audioclip,transform.position);
        }
           
       
        // StopFlag=true;
    }
    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthHP>()!=null)
        {
            if(!collision.gameObject.GetComponent<HealthHP>().noeffect_flag)
            {
                setUnActive(0.15f);
                // blast_object.GetComponent<Transform>().position=collision.contacts[0].point;
                // blast_canSeeDuringTime(0.2f);
                explode_man.blastI_at_V3(1,collision.contacts[0].point);
                AudioSource.PlayClipAtPoint(audioclip,transform.position);
            }
        }else
        {
            setUnActive(0.15f);
            // blast_object.GetComponent<Transform>().position=collision.contacts[0].point;
            // blast_canSeeDuringTime(0.2f);
            explode_man.blastI_at_V3(1,collision.contacts[0].point);
            AudioSource.PlayClipAtPoint(audioclip,transform.position);
        }
           
       
        // StopFlag=true;
    }
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthHP>()!=null)
        {
            if(!collision.gameObject.GetComponent<HealthHP>().noeffect_flag)
            {
                setUnActive(0.15f);
                // blast_object.GetComponent<Transform>().position=collision.contacts[0].point;
                // blast_canSeeDuringTime(0.2f);
                explode_man.blastI_at_V3(1,collision.contacts[0].point);
                AudioSource.PlayClipAtPoint(audioclip,transform.position);
            }
        }else
        {
            setUnActive(0.15f);
            // blast_object.GetComponent<Transform>().position=collision.contacts[0].point;
            // blast_canSeeDuringTime(0.2f);
            explode_man.blastI_at_V3(1,collision.contacts[0].point);
            AudioSource.PlayClipAtPoint(audioclip,transform.position);
        }
           
       
        // StopFlag=true;
    }
	//开始接触
    void OnTriggerEnter(Collider collider) {
		// Debug.Log("collider.transform.position"+collider.transform.position); 
        // Debug.Log("开始接触");
    }
    // 接触结束
    void OnTriggerExit(Collider collider) {
        // Debug.Log("接触结束");
    }
    // 接触持续中
    void OnTriggerStay(Collider collider) {
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

        if(trailingMesh2!=null)
        {
            trailingMesh2.setVisual();
        }
    
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
        if(trailingMesh2!=null)
        {
            trailingMesh2.setUnVisual();
        }
       
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

                if(trailingMesh2!=null)
                {
                    trailingMesh2.setUnVisual();
                }
             
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
