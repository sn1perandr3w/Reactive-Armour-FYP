using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initPlayerPrefs : MonoBehaviour {

	// Use this for initialization
	void Start () {

		PlayerPrefs.SetInt("civSaved",0);
		PlayerPrefs.SetInt("combatEffectiveness",0);
		PlayerPrefs.SetInt("level",0);
		PlayerPrefs.SetInt("experience",0);
		if (PlayerPrefs.GetInt ("difficulty") == null) {
			PlayerPrefs.SetInt ("difficulty", 0);
		}

		print("DIFFICULTY NO = " + PlayerPrefs.GetInt("difficulty"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
