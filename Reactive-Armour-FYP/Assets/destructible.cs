using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructible : MonoBehaviour {

	public GameObject explosionSound;

	public int health;
	public int healthLimit;
	public string destructibleName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void lowerHealth(int amount)
	{
		health = health - amount;
		if (health <= 0) 
		{
			if (gameObject.GetComponent<civEntity>() != null) 
			{
				GameObject civController = GameObject.Find("civController");
				civController.GetComponent<civController>().lowerCivAmount(this.gameObject.GetComponent<civEntity>().civAmount());
			}
			GameObject e = (GameObject)Instantiate(explosionSound, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}

		print ("Object: " + gameObject.name + " health = " + health);
	}

	public string getDestructibleName()
	{
		return destructibleName;
	}

	public void increaseHealth( int amount)
	{
		if ((health + amount) < healthLimit) {
			health = health + amount;
		} else 
		{
			health = healthLimit;
		}
	}
}
