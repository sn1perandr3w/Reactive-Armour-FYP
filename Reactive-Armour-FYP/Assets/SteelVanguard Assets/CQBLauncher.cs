using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CQBLauncher : MonoBehaviour {

	//Declaration of values/objects

	public GameObject ball;
	float time = 0.0f;
	int health = 50;

	void Start () {
	}


	void Update ()
	{

		//Destroys turret + NPC it's attached to upon health running out.
		if (health < 1) {

			Destroy(transform.parent.gameObject);
			Destroy(this.gameObject);

		}

		//Gets distance to player from chestTarget in player.
		float distance = Vector3.Distance (transform.position, GameObject.Find ("chestTarget").transform.position);
		//Gets target on player (Empty object located in chest due to standard PlayerController making the turret aim at their feet


		//Firing procedure.
		if (distance < 120.0f) {
			transform.LookAt (GameObject.Find("chestTarget").transform.position);
			time += Time.deltaTime;

				if (time >= 2) {
					time = 0.0f;


					GameObject g = (GameObject)Instantiate (ball, transform.position + transform.forward * 3.0f, Quaternion.identity);	
					g.GetComponent<Rigidbody> ().AddForce (transform.forward * 4000);
					Destroy (g, 4);

				}
			}
		}



	//Lowers the turret's health when hit with a projectile.

	public void lowerHealth(int removal)
	{
		health = health - removal;
		print ("Took " + removal + " damage.");
	}
}
