using UnityEngine;
using System.Collections;

//Used just for ammo pickup. Could probably be cleaned up and inserted into healthScript

public class manageCollisionWithPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision hit)
	{
		print ("Collided with " + hit.gameObject.name);	
		GetComponent<ManageWeapons>().manageCollisions (hit);
	}
}
