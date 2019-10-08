using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//Used for sniper turrets to ensure they can shoot at long range.

public class AILauncher : MonoBehaviour {

	//Declaration of values/objects.

	public GameObject ball;
	float time = 0.0f;
	float timeToFire = 3.0f;
	public bool firing = false;
	Camera c;
	int health = 100;
	// Use this for initialization
	void Start () {
		c = transform.GetChild(0).GetComponent<Camera> ();
		c.depth = -1;
	}


	void Update ()
	{

		GameObject.Find ("UISniper").GetComponent<Text> ().text = timeToFire + "";
		GameObject.Find ("UISniper2").GetComponent<Text> ().text = "Time until next shot:";
		//Destroys turret + NPC it's attached to upon health running out.
		if (health < 1) {
			if (firing == true) {
				//Prevents death of turret locking up the sequence of firing with other snipers.
				//Bugged at the moment. The sniper can potentially kill itself while aiming downward. Works outside of that situation, however, even if player kills sniper.
				healthScript.squadFire = true;
			}
				Destroy(transform.parent.gameObject);
				Object.Destroy (this.gameObject);
		}

		//Gets distance to player from chestTarget in player.
		float distance = Vector3.Distance (transform.position, GameObject.Find ("chestTarget").transform.position);


		//Gets target on player (Empty object located in chest due to standard PlayerController making the turret aim at their feet.
		if (distance < 120.0f) {

			transform.LookAt (GameObject.Find ("chestTarget").transform.position);
			time += Time.deltaTime;


			timeToFire -= Time.deltaTime;

			//Allows this turret to fire and no other sniper turret to do so. 
			//Prevents player from being overwhelmed with an unfair amount of sniper shots.
			if (healthScript.squadFire == true && firing == false) {
				//print ("Setting turret to fire");
				firing = true;
				healthScript.squadFire = false;
				time = 0.0f;
	
			}

			//Firing procedure when the turret has been allowed to fire.

			if (firing == true) {
				c.depth = 1;
				if (time >= 3 ) {
					GameObject.Find ("UISniper").GetComponent<Text> ().text = timeToFire + "";
					GameObject.Find ("UISniper2").GetComponent<Text> ().text = "Time until next shot:";

					time = 0.0f;
					timeToFire = 3.0f;

					GameObject g = (GameObject)Instantiate (ball, transform.position + transform.forward * 12.0f, Quaternion.identity);	
					g.GetComponent<Rigidbody> ().AddForce (transform.forward * 6000);
					Destroy (g, 8);
					firing = false;
					healthScript.squadFire = true;
				}
			}
		}

		//Makes it so that another sniper's camera can take over when this one has finished firing, as the player will be able to see an enemy sniper's camera
		//when it is about to fire at the player.

		if (firing == false) 
		{
			c.depth = -1;
			GameObject.Find ("UISniper").GetComponent<Text> ().text = "";
			GameObject.Find ("UISniper2").GetComponent<Text> ().text = "";
		}

		//Makes the sniper let go of its allotted firing time if the player is too far away to shoot.

		if (distance > 120.0f & firing == true) 
		{
			firing = false;
			healthScript.squadFire = true;
			timeToFire = 3.0f;
		}


	}

	//Lowers the turret's health when hit with a projectile.

	public void lowerHealth(int removal)
	{
		health = health - removal;
		print ("Took " + removal + " damage.");
	}
}
