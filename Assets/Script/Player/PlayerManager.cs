using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	public AudioClip audioclip_P;
	public AudioClip audioclip_A;
	public GameObject lose_text;
	public GameObject g1;
	public GameObject g2;
	// Use this for initialization
	private ExplodeManager explodeM;
	private bool lose_flag=false;
	void Start () {
		if(explodeM==null)
		{
			explodeM=GameObject.FindGameObjectWithTag("ExplodeManager").GetComponent<ExplodeManager>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(g1.GetComponent<HealthHP>().health_value<=0)
		{
			if(g1.GetComponent<ZhaGuCollider4>().ani.gameObject.activeSelf)
			{
				explodeM.blastI_at_V3(0,g1.transform.position+Vector3.up*1f);
				if(audioclip_A!=null)
				{
					AudioSource.PlayClipAtPoint(audioclip_A,transform.position);
				}
			}
			g1.GetComponent<ZhaGuCollider4>().ani.gameObject.SetActive(false);
			g1.GetComponent<ZhaGuCollider4>().enabled=false;
			lose_flag=true;
		}

		if(g2.GetComponent<HealthHP>().health_value<=0)
		{
			if(g2.GetComponent<WuMingYiDuanCollider4>().ani.gameObject.activeSelf)
			{
				explodeM.blastI_at_V3(0,g2.transform.position+Vector3.up*1f);
				if(audioclip_A!=null)
				{
					AudioSource.PlayClipAtPoint(audioclip_A,transform.position);
				}
			
			}
			g2.GetComponent<WuMingYiDuanCollider4>().ani.gameObject.SetActive(false);
			g2.GetComponent<WuMingYiDuanCollider4>().enabled=false;
			lose_flag=true;
		}
			
		lose_process();//战败后继处理
	}


	///<summary>战败后继处理</summary>
	void lose_process()
	{
		if(lose_flag)
		{
			lose_text.SetActive(true);
			if(Input.GetKey(KeyCode.Return))
			{
				Cursor.visible=true;
				Cursor.lockState=CursorLockMode.None;
				SceneManager.LoadScene("Load_Scene");
			}
		}
	}

	///<summary>接触变身</summary>
	 void OnTriggerStay(Collider collider) {
		if(collider.tag.Equals("Player"))
		{
			GetComponent<Collider>().enabled=false;
			GetComponent<Renderer>().enabled=false;
			g1.SetActive(false);
			g2.SetActive(true);
			if(audioclip_P!=null)
			{
				AudioSource.PlayClipAtPoint(audioclip_P,transform.position);
			}
		}
		
    }
}
