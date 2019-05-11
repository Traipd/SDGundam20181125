using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectList {

    private GameObject[] list=new GameObject[0];
	public int Length=0;

	///<summary>从队头加入一个对象</summary>
	public void setIn(GameObject g)
	{
		GameObject[] list2=new GameObject[list.Length+1];
		list2[0]=g;
		for(int i=0;i<list.Length;i++)
		{
            list2[i+1]=list[i];
		}
		list=list2;
		Length=Length+1;
	}

	///<summary>从队尾移出一个对象</summary>
	public GameObject putOut()
	{
		if(Length>0)
		{
			GameObject[] list2=new GameObject[list.Length-1];
			GameObject outobject=list[list.Length-1];
			for(int i=0;i<list.Length-1;i++)
			{
				list2[i]=list[i];
			}
			list=list2;
			Length=Length-1;

			return outobject;
		}else
		{
			// Debug.Log("GameObjectList.class: the list has nothing");
			return null;
		}
	}

	///<summary>移出某个元素（从0开始）</summary>
	public	GameObject putOutI(int i)
	{
        GameObject outobject=list[i];
		
		GameObject[] list2=new GameObject[list.Length-1];
		for(int j=0;j<i;j++)
		{
			list2[j]=list[j];
		}
		for(int j=i;j<=list.Length-1;j++)
		{
			if(j<=list.Length-2)
			{
				list2[j]=list[j+1];
			}
		}
		list=list2;
		Length=Length-1;
		return outobject;
	}
	

}
