using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gameOverScreenScript : MonoBehaviour {


	//Changes scene to specified scene when button is pressed.
	public void continueGame()
	{
		SceneManager.LoadScene(PlayerPrefs.GetString("scene"));
	}

	public void goToMap()
	{
		SceneManager.LoadScene("mapScreen");
	}

	public void exitToMenu()
	{
		SceneManager.LoadScene("splashScreen");
	}

}