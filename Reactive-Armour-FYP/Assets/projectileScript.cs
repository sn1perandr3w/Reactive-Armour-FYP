using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class projectileScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll)
	{
		
		//Removes health from Player.

		if (coll.gameObject.tag == "player") {

			coll.gameObject.GetComponent<playerController>().lowerHealth(20);
		}

		if (coll.gameObject.tag == "destructible") {
			print ("HITTING CIVILIANS");
			coll.gameObject.GetComponent<destructible>().lowerHealth(20);
		}
			
		Object.Destroy (this.gameObject);
	}
		

}
