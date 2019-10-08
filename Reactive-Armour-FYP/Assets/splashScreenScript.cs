using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class splashScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	//Changes scene to specified scene when button is pressed.
	public void newGame()
	{
		SceneManager.LoadScene("safeZone");
	}

	public void continueGame()
	{
		SceneManager.LoadScene("mapScreen");
	}

	public void options()
	{
		SceneManager.LoadScene("optionsScreen");
	}

	public void help()
	{
		SceneManager.LoadScene("instructionScreen");
	}

	public void helpReturn()
	{
		SceneManager.LoadScene("splashScreen");
	}

	public void exitGame()
	{
		
	}

}