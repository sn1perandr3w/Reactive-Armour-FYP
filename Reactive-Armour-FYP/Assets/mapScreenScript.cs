using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mapScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void F13()
	{
		SceneManager.LoadScene("safeZone");
	}

	public void R05()
	{
		SceneManager.LoadScene("town");
	}


}
