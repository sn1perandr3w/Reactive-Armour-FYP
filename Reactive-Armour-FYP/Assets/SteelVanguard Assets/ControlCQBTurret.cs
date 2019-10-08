using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlCQBTurret : MonoBehaviour {

	//Declaration of values/objects.

	int health = 50;
	int hitInt;
	int pathIndex =1;
	int wpNameSelectNum;

	Animator anim;


	AnimatorStateInfo info; //to determine the state

	void Start () {
		anim = GetComponent<Animator> ();

		//Sets random waypoint
		wpNameSelectNum = Random.Range(1,3);

	}


	void Update () {

		//Gets position of player. (Uses chestTarget empty object inside player.)

		float distance = Vector3.Distance (transform.position, GameObject.Find ("chestTarget").transform.position);

		info = anim.GetCurrentAnimatorStateInfo (0);


		//ANIMATOR STATES
		//This is for the followPlayer state in the animator.

		if (info.IsName ("followPlayer")) 
		{
			GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = GameObject.Find ("chestTarget").transform.position;
			GetComponent<UnityEngine.AI.NavMeshAgent> ().Resume ();//Unity5  version
			//GetComponent<NavMeshAgent> ().isStopped = false;//Unity5  version
			if(distance > 60.0f) anim.SetTrigger("stopFollow");

		}

		//CutOffPlayer makes the turret move towards the only exit in the level in an attempt to stop the player leaving once they have all the power cells.

		if (info.IsName ("cutOffPlayer")) 
		{
			print ("Cutting Off Player");
			GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = GameObject.Find ("OpenDoor").transform.position;
			GetComponent<UnityEngine.AI.NavMeshAgent> ().Resume ();
		}


		//Used for hit animation, also returns state to previous state, depending on int assigned to it. each int corresponds to a state.

		if (info.IsName ("hit")) 
		{
			if (hitInt == 1) 
			{
				anim.SetTrigger ("stopHitFollow");
			}
			if (hitInt == 2) 
			{
				anim.SetTrigger ("stopHit");
			}
			if (hitInt == 3) 
			{
				anim.SetTrigger ("StopHitCutOff");
			}
		}

		//Patrolling state makes the turret move to any number of set waypoints set on the map.

		if (info.IsName ("patrol")) 
		{
			if (distance < 60.0f) 
			{
				anim.SetTrigger ("startFollow");
			}
			if (healthScript.powerCells > 3) 
			{
				anim.SetTrigger("cutOff");
			}



			string wpNameSelect = "not assigned";
			string wpName = "Waypoint (" + pathIndex + ")";
			string wpName2 = "WaypointALT (" + pathIndex + ")";

			//Determines if the turret will follow Waypoint or WaypointALT.
			//Waypoints are decided randomly from 2 sets of waypoints. (Waypoint and WaypointALT)

			if (wpNameSelectNum == 1) {
				wpNameSelect = wpName;
				print (wpNameSelect);
			}
			if (wpNameSelectNum == 2) {
				wpNameSelect = wpName2;
			}
				

			GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = GameObject.Find (wpNameSelect).transform.position;
			GetComponent<UnityEngine.AI.NavMeshAgent> ().Resume ();//Unity5  version

			float distanceToWP = Vector3.Distance(transform.position, GameObject.Find (wpNameSelect).transform.position);
			//GetComponent<NavMeshAgent> ().isStopped = false;//Unity5  version

			if (distanceToWP < 2.0f) pathIndex++;
			if (pathIndex > 4) {
				pathIndex = 1;
				wpNameSelectNum = Random.Range (1, 3);
			}

		}


		//Destroys object when health is gone.
		if (health < 1) {
			enemySpawn.totalEnemyAmount = enemySpawn.totalEnemyAmount - 1;
			Object.Destroy (this.gameObject);
		}
	}

	//Removes health from Object. Also assigns hitInt to determine which state it returns to once in the hit state for the animator controller.
	public void lowerHealth(int removal)
	{
		health = health - removal;
		print ("CQB Turret took " + removal + " damage.");

		if (info.IsName ("followPlayer")) 
		{
			anim.SetTrigger("getHitFollow");
			hitInt = 1;
		}
		if (info.IsName ("patrol")) 
		{
			anim.SetTrigger("getHit");
			hitInt = 2;
		}
		if (info.IsName ("cutOffPlayer")) 
		{
			anim.SetTrigger("getHitCutOff");
			hitInt = 3;
		}

	}


}