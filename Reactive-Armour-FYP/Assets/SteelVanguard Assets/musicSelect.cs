using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Used to determine whether music object is spawned or not depending on the playerpref musicToggle. This is done if the player wants to listen to music.
public class musicSelect : MonoBehaviour {
	public GameObject Music;
	// Use this for initialization
	void Start () {

		int localToggle = PlayerPrefs.GetInt ("musicToggle");

		//if (localToggle == 1) {
			Instantiate (Music, transform.position, Quaternion.identity);
		//}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
