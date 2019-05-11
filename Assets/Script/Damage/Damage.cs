using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

	public float damage_value=1;
	public int damage_level=0;
    public float damage_down=20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthHP>()!=null)
        {
            collision.gameObject.GetComponent<HealthHP>().onDamage(damage_value,damage_level,damage_down);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthHP>()!=null)
        {
            collision.gameObject.GetComponent<HealthHP>().onDamage(damage_value,damage_level,damage_down);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.GetComponent<HealthHP>()!=null)
        {
            collision.gameObject.GetComponent<HealthHP>().onDamage(damage_value,damage_level,damage_down);
        }
    }
	 // 开始接触
    void OnTriggerEnter(Collider collider) {
        if(collider.GetComponent<HealthHP>()!=null)
        {
            collider.GetComponent<HealthHP>().onDamage(damage_value,damage_level,damage_down);
        }
		// Debug.Log("collider.transform.position"+collider.transform.position);
		

		//  Destroy(this.gameObject);
         // 销毁当前游戏物体
        // Debug.Log("开始接触");
    }

    // 接触结束
    void OnTriggerExit(Collider collider) {
        if(collider.GetComponent<HealthHP>()!=null)
        {
            collider.GetComponent<HealthHP>().onDamage(damage_value,damage_level,damage_down);
        }
        // Debug.Log("接触结束");
    }

    // 接触持续中
    void OnTriggerStay(Collider collider) {
        if(collider.GetComponent<HealthHP>()!=null)
        {
            collider.GetComponent<HealthHP>().onDamage(damage_value,damage_level,damage_down);
        }
        // Debug.Log("接触持续中");
    }
}
