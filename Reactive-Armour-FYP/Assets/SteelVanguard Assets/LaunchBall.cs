using UnityEngine;
using System.Collections;

public class LaunchBall : MonoBehaviour {

	//Declaration of object.
	public GameObject ball;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update ()
	{
		//Launches projectile from Launcher, allowing the player to attack.
		if (Input.GetKeyDown (KeyCode.P))
		{
			GameObject g = (GameObject)Instantiate(ball, transform.position, Quaternion.identity);	
			g.GetComponent<Rigidbody> ().AddForce (transform.forward * 4000);
			Destroy (g, 1);
		}

	}
}