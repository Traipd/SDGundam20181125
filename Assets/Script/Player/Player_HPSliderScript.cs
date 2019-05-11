using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_HPSliderScript : MonoBehaviour {
	public HealthHP hp;
	private Slider slider;
	private float max_hpvalue;

	// Use this for initialization
	void Start () {
		slider=GetComponent<Slider>();
		max_hpvalue=hp.health_value;
	}
	
	// Update is called once per frame
	void Update () {
		slider.value=hp.health_value/max_hpvalue;
	}
}
