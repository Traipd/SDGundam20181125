using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatList  {

	private float[] list=new float[0];
	public int Length=0;

	///<summary>从队头加入一个对象</summary>
	public void setIn(float g)
	{
		float[] list2=new float[list.Length+1];
		list2[0]=g;
		for(int i=0;i<list.Length;i++)
		{
            list2[i+1]=list[i];
		}
		list=list2;
		Length=Length+1;
	}

	///<summary>从队尾移出一个对象</summary>
	public float putOut()
	{
		if(Length>0)
		{
			float[] list2=new float[list.Length-1];
			float outobject=list[list.Length-1];
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
			return 0;
		}
	}

	///<summary>移出某个元素（从0开始）</summary>
	public	float putOutI(int i)
	{
        float outobject=list[i];
		
		float[] list2=new float[list.Length-1];
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
	///<summary>修改某个元素（从0开始）</summary>
	public void writeI(int i,float f)
	{
		list[i]=f;
	}

	///<summary>读取某个元素（从0开始）</summary>
	public float readI(int i)
	{
		return list[i];
	}
}
