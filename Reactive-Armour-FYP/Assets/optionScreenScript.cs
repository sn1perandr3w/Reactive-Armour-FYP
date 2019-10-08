using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class optionScreenScript : MonoBehaviour {

	//Changes scene to specified scene when button is pressed.


	public void changeDifficulty()
	{
		if (PlayerPrefs.GetInt ("difficulty") == 1) {
			PlayerPrefs.SetInt ("difficulty", 0);
			GameObject.Find ("Button").GetComponentInChildren<Text>().text = "Difficulty: Hard";
		}
		else
		{
			PlayerPrefs.SetInt ("difficulty", 1);
			GameObject.Find ("Button").GetComponentInChildren<Text>().text = "Difficulty: Easy";
		}

		print("DIFFICULTY NO = " + PlayerPrefs.GetInt("difficulty"));

	}

	public void exitToMenu()
	{
		SceneManager.LoadScene("splashScreen");
	}

}
