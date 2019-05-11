using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fly_Slider_hide : MonoBehaviour {

    Slider slider;
	// Use this for initialization
	void Start () {
		slider=this.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(slider.value==1)
		{
			GetComponent<CanvasGroup>().alpha=0;
		}else{
			GetComponent<CanvasGroup>().alpha=1;
		}
	}
}
