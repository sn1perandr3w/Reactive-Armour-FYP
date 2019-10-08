using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProjectile : MonoBehaviour {

	//declaration of object.
	public GameObject explosion;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision coll)
	{
		//Aids in detecting object collided with.
		string name = coll.collider.gameObject.name;
		string tagName = coll.collider.gameObject.tag;


		//Removes player health if projectile hits the player. (Weaker than standard projectiles fired from sniper or player.
		if (tagName == "Player") {

			coll.gameObject.GetComponent<healthScript>().lowerHealth(10);
		}
			
		//Causes explosion upon collision.
		Instantiate (explosion, transform.transform.position, Quaternion.identity);
		Object.Destroy (this.gameObject);
	}
}
