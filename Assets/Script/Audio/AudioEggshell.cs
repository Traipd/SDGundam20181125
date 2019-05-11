using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEggshell : MonoBehaviour {

	public AudioClip eggshell_audioclip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider collider) {
		if(collider.tag.Equals("Player"))
		{	
			AudioSource a=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
			a.clip=eggshell_audioclip;
			a.Play();
			gameObject.GetComponent<Collider>().enabled=false;
			StartCoroutine(stop_until());
		}
		
    }
	IEnumerator stop_until()
	{
		float t=0;
		while (t<5f)
		{
			t+=Time.fixedDeltaTime;
			yield return null;
		}
		gameObject.GetComponent<Collider>().enabled=true;
		yield return null;
	}
}
