using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	//Declares object.
	public GameObject explosion;

	void Start () {
	
	}

	void Update () {
	
	}

	//Detects collision in regards to triggers.

	void OnTriggerEnter(Collider call)
	{	
		print ("trigger Entered");

		//Removes health from Sniper turret.
		if (call.gameObject.tag == "enemySniper") {

			call.gameObject.GetComponent<AILauncher>().lowerHealth(50);
		}
		//Removes health from CQB turret.
		if (call.gameObject.tag == "enemyCQB") {

			call.gameObject.GetComponent<CQBLauncher>().lowerHealth(50);
		}

		//Removes projectile once trigger has been hit.
		Object.Destroy (this.gameObject);
	}
		

	//Detects collision.

	void OnCollisionEnter(Collision coll)
	{
		//Aids in collision detection and methods stemming from it.
		string name = coll.collider.gameObject.name;
		string tagName = coll.collider.gameObject.tag;

		print(name);
		print(tagName);

		//Removes health from Player.

		if (tagName == "Player") {

			coll.gameObject.GetComponent<healthScript>().lowerHealth(50);
		}

		/*if (tagName == "enemyCQB") {

			coll.gameObject.GetComponent<ControlCQBTurret>().lowerHealth(50);
		}*/

		//Creates explosion and removes projectile.
		Instantiate (explosion, transform.transform.position, Quaternion.identity);
		Object.Destroy (this.gameObject);
	}
}
