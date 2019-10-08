using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCellSpawn : MonoBehaviour {
	//Declaration of ints and objects.
	public GameObject PowerCell;
	int spawnOrder;


	// Use this for initialization
	void Start()
	{

	}

	void Update()
	{
		spawnOrder = Random.Range(1,3);
		spawnCells();
	}

	//Spawns power cells at random locations. the spawnOrder is used to prevent all of the spawnpoints spawning cells before the maximum amount of power cells
	//per map has been reached. (It is 4 for scene 2.)

	void spawnCells()
	{
		if (spawnOrder == 2) {
			if (healthScript.powerCellSpawns < healthScript.powerCellMax) {
				Instantiate (PowerCell, transform.position + Vector3.right, Quaternion.identity);
				healthScript.powerCellSpawns = healthScript.powerCellSpawns + 1;
			}
		}
	}
}