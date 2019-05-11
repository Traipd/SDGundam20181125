using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_SceneScript : MonoBehaviour {

	[Tooltip("下一个场景的名称（字符串）")]
	public string SceneName_next;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onClick(GameObject g)
	{
		SceneManager.LoadScene(SceneName_next);
	}
}
