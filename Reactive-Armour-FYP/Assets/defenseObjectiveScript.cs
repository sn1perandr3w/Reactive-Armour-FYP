using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defenseObjectiveScript : MonoBehaviour {
	
		int enemyCount = 0;

		public GameObject player;

		// Use this for initialization
		void Start () {


			player = GameObject.FindGameObjectWithTag ("player");



			player.GetComponent<playerController>().ableToEscape = true;

			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
			{	
				enemyCount++;
			}
		}

		// Update is called once per frame
		void Update () {

			enemyCount = 0;
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
			{	
				enemyCount++;
			}

			if(enemyCount == 0)
			{
				player.GetComponent<playerController> ().ableToEscape = true;

				player.GetComponent<playerController> ().objective = "Escape (ESC)";
			}

		}
	}