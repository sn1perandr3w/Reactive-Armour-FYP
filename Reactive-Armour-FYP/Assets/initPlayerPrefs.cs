using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initPlayerPrefs : MonoBehaviour {

	// Use this for initialization
	void Start () {

		PlayerPrefs.SetInt("civSaved",0);
        PlayerPrefs.SetInt("F13CLEAR", 0);
        PlayerPrefs.SetInt("R05CLEAR", 0);
        PlayerPrefs.SetInt("D01CLEAR", 0);
        PlayerPrefs.SetInt("U63CLEAR", 0);
        PlayerPrefs.SetInt("S31CLEAR", 0);
        PlayerPrefs.SetInt("allyMechsFound", 0);

        PlayerPrefs.SetInt("healthSaved", 100);
        PlayerPrefs.SetInt("ammoSaved", 100);




        if (PlayerPrefs.GetInt ("difficulty") == null) {
			PlayerPrefs.SetInt ("difficulty", 0);
		}

		print("DIFFICULTY NO = " + PlayerPrefs.GetInt("difficulty"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
