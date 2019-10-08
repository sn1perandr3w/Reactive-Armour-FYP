using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to spawn the player at corresponding spawnpoint without duplicating player.

public class playerSpawnPoint2 : MonoBehaviour {

	public GameObject Player;
	void Start () {
		//spawnLocation will be different depending on the last area the player was in.
		int spawnLocation = PlayerPrefs.GetInt("spawnLocation");

		if (spawnLocation == 1) {
			Instantiate (Player, transform.position, Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}