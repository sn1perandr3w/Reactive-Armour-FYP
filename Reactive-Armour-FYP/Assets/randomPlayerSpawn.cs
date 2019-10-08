using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPlayerSpawn : MonoBehaviour {

	public GameObject player;
	public GameObject aimtarget;
	public GameObject playerCamera;

	static int spawnNum;

	public int localSpawnNum;

	public List<GameObject> playerSpawns;

	// Use this for initialization
	void Start () {

		foreach (GameObject playerSpawn in GameObject.FindGameObjectsWithTag("playerSpawn")) 
		{
			playerSpawns.Add (playerSpawn);
		}


		spawnNum = Random.Range(0,playerSpawns.Count+1);

		if (spawnNum == localSpawnNum) 
		{
			player.transform.position = this.transform.position;
			aimtarget.transform.position = this.transform.position + transform.forward * 10.0f;
			playerCamera.transform.position = this.transform.position - transform.forward * 10.0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
