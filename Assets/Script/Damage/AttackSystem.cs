using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour {
	public CameraScript camerascript;//发射射线的摄像机脚本

	public AduioListPlayScript aduioplayer;
    [System.Serializable]
	public struct AttackObject_list{
		public GameObject attackObject;//攻击用物体
	    public Transform attack_starttransform;//攻击用物体的发射起点
		public float attackObject_activetime;//攻击用物体的最大存活时间
		public bool has_exit;//攻击用物体是否已存在（不需要创建）
		public int aduio_number;
	}

	public AttackObject_list[] attackObject_list;//攻击用物体的主要信息
	GameObjectList[] attackObject_listbuff;//已停止活动的攻击用物体缓存池
	GameObjectList[] attackObject_active_listbuff;//活动中的攻击用物体缓存池
	FloatList[] attackObject_active_listtime;//活动中的攻击用物体计时
	public int attackObject_number=0;//选择的是哪种攻击物体
    
	bool attack_flag=false;
	// Use this for initialization
	void Start () {//初始化
	   attackObject_listbuff=new GameObjectList[attackObject_list.Length];
	   for(int i=0;i<attackObject_listbuff.Length;i++)
	   {
		   attackObject_listbuff[i]=new GameObjectList();
	   }

	   attackObject_active_listbuff=new GameObjectList[attackObject_list.Length];
	   for(int i=0;i<attackObject_listbuff.Length;i++)
	   {
		   attackObject_active_listbuff[i]=new GameObjectList();
	   }

		attackObject_active_listtime=new FloatList[attackObject_list.Length];
		for(int i=0;i<attackObject_listbuff.Length;i++)
	   {
		   attackObject_active_listtime[i]=new FloatList();
	   }
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		
		if(attack_flag)//false
		{
			attack_flag=false;
			if(attackObject_list[attackObject_number].has_exit)
			{
				attackObject_list[attackObject_number].attackObject.GetComponent<AttackSystemInObject>().setFireFlag(true);
				if(attackObject_list[attackObject_number].attackObject.GetComponent<AttackSystemInObject>().getFireFlag())
				{
					attackObject_active_listtime[attackObject_number].setIn(0f);
					aduio_play();
				}
			}else
			{
				GameObject g=attackObject_listbuff[attackObject_number].putOut();//子弹队列操作
				if(g==null)
				{
					g=(GameObject)Instantiate(attackObject_list[attackObject_number].attackObject,Vector3.zero,Quaternion.identity);
				}
				
				g.GetComponent<Transform>().position=attackObject_list[attackObject_number].attack_starttransform.position;

				Vector3 f=camerascript.getRay_point()-attackObject_list[attackObject_number].attack_starttransform.position;//子弹方向的计算
				Vector3 r=attackObject_list[attackObject_number].attack_starttransform.right;
				Vector3 u=Vector3.Cross(f,r);
				g.GetComponent<Transform>().rotation=Quaternion.LookRotation(f,u);

				
				g.GetComponent<AttackSystemInObject>().setFireFlag(true);//请在重置位置后再设置可见，拖尾效果的初始位置受次影响
				attackObject_active_listbuff[attackObject_number].setIn(g);
				attackObject_active_listtime[attackObject_number].setIn(0f);
				
				aduio_play();
			}
	
		}

		attackObject_Recover();
	}
	///<summary>子弹发射或实例音效</summary>
	void aduio_play()
	{
		// if(attackObject_number==0)
		// {
			aduioplayer.play_attack_audio(attackObject_list[attackObject_number].aduio_number,transform.position);
		// }
		// if(attackObject_number==1)
		// {
		// 	aduioplayer.play_attack_audio(2,transform.position);
		// }
		// if(attackObject_number==2)
		// {
		// 	aduioplayer.play_attack_audio(3,transform.position);
		// }
		// if(attackObject_number==3||attackObject_number==4)
		// {
		// 	aduioplayer.play_attack_audio(4,transform.position);
		// }
	}

	///<summary>子弹缓存池按时自动回收</summary>
    void attackObject_Recover()
	{
		for(int i=0;i<attackObject_list.Length;i++)//每种
		{
			for(int j=0;j<attackObject_active_listtime[i].Length;j++)//每个运行的子弹
			{
				float ijtime=attackObject_active_listtime[i].readI(j);
				ijtime+=Time.fixedDeltaTime;
				if(ijtime>attackObject_list[i].attackObject_activetime)
				{
					if(attackObject_list[i].has_exit)
					{
						attackObject_list[i].attackObject.GetComponent<AttackSystemInObject>().setStopFlag(true);
						attackObject_active_listtime[i].putOutI(j);
					}else
					{
						GameObject recover=attackObject_active_listbuff[i].putOutI(j);
						recover.GetComponent<AttackSystemInObject>().setStopFlag(true);
						attackObject_listbuff[i].setIn(recover);
						attackObject_active_listtime[i].putOutI(j);
					}

				}else
				{
					attackObject_active_listtime[i].writeI(j,ijtime);
				}
			}
		}
	}
	///<summary>设置子弹的种类</summary>
	public void set_AttackObject_number(int i)
	{
		attackObject_number=i;
	}
	///<summary>设置攻击flag</summary>
	public void set_Attack_flag(bool f)
	{
		attack_flag=f;
	}
    ///<summary>立即回收某种跟角色绑定的攻击物体</summary>
	public void attackObject_Recover_one(int i)
	{
		for(int j=0;j<attackObject_active_listtime[i].Length;j++)//每个运行的子弹
		{
			attackObject_list[i].attackObject.GetComponent<AttackSystemInObject>().setStopFlag(true);
			attackObject_active_listtime[i].putOutI(j);
		}
		
	}
}
