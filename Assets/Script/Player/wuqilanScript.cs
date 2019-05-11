using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wuqilanScript : MonoBehaviour {

	public GameObject[] WuQi;

    public float up_y=100;
	public float down_y=30;


	// Use this for initialization
	void Start () {
		//Debug.Log(WuQi_1.GetComponent<RectTransform>().anchoredPosition);
	}
	
	// Update is called once per frame
	void Update () {

		// if(Input.GetKeyDown(KeyCode.Alpha1))
		// {
		// 	setAllDefult();setOnesUp(0);
		// }
		// if(Input.GetKeyDown(KeyCode.Alpha2))
		// {
		// 	setAllDefult();setOnesUp(1);
		// }
		// if(Input.GetKeyDown(KeyCode.Alpha3))
		// {
		// 	setAllDefult();setOnesUp(2);
		// }

	}

    public void setOnesUp(int i)
	{
        setAllDefult();
		Vector2 fr=WuQi[i].GetComponent<RectTransform>().anchoredPosition;
		WuQi[i].GetComponent<RectTransform>().anchoredPosition=new Vector2(fr.x,up_y);
	}
    void setAllDefult()
	{
		for(int i=0;i<WuQi.Length;i++)
		{
			Vector2 fr=WuQi[i].GetComponent<RectTransform>().anchoredPosition;
			WuQi[i].GetComponent<RectTransform>().anchoredPosition=new Vector2(fr.x,down_y);
		}
	}
}
