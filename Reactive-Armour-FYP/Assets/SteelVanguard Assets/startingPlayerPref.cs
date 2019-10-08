using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class startingPlayerPref : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//Sets PlayerPrefs at the start of the game, either from the splash, gameover or victory screens.
		PlayerPrefs.SetInt("spawnLocation",0);
		PlayerPrefs.SetInt ("powerCells", 0);
		PlayerPrefs.SetInt ("health", 100);
		PlayerPrefs.SetInt ("musicToggle", 1);
		PlayerPrefs.SetFloat ("Time", 0.0F);
		PlayerPrefs.SetInt("gunAmmo",10);
		PlayerPrefs.SetInt("autoGunAmmo",15);
		PlayerPrefs.SetInt("swordAmmo",8);
	}

	void Update () {
		if (PlayerPrefs.GetInt ("musicToggle") == 1) {
			GameObject.Find ("Button2").GetComponentInChildren<Text> ().text = "Toggle Music: True";	
		} else 
		{
			GameObject.Find ("Button2").GetComponentInChildren<Text> ().text = "Toggle Music: False";
		}

	}


	//Used to determine whether music is played.
	public void musicToggle()
	{
		int localToggle = PlayerPrefs.GetInt ("musicToggle");

		if (localToggle == 1) {
			PlayerPrefs.SetInt ("musicToggle", 0);
		} else if (localToggle == 0) 
		{
			PlayerPrefs.SetInt ("musicToggle", 1);
		}
	}
}
