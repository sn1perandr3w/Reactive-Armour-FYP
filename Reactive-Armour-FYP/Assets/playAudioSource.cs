using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudioSource : MonoBehaviour {

	//AudioSource audiosource;
	public float timeUntilDestruction;
	// Use this for initialization
	void Start () {
		//audiosource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(this, timeUntilDestruction);
	}
}
