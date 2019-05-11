using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Load_SceneScript : MonoBehaviour {

	[Tooltip("下一个场景的名字（字符串）")]
	public string SceneName_next;
	[Tooltip("进度条UI（Slider）")]
	public Slider slider;
	[Tooltip("读取进度数值UI（Text）")]
	public Text text;
	AsyncOperation async=null;
	bool allow_finish_flag=false;
	// Use this for initialization
	void Start () {

		StartCoroutine("LoadScene");
	}
	void Update () {}
	IEnumerator LoadScene()
	{
		async=SceneManager.LoadSceneAsync(SceneName_next);
		async.allowSceneActivation=false;
		while(!async.isDone)
		{
			slider.value=async.progress;
			text.text=async.progress*100+"%";
			if(allow_finish_flag)
			{async.allowSceneActivation=true;}

			yield return 0;
		}
	}
	public void OnClick()
	{
		allow_finish_flag=true;
	}
}
