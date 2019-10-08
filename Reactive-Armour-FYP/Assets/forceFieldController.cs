using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceFieldController : MonoBehaviour {

	public int generatorCount = 0;

	public GameObject player;

	// Use this for initialization
	void Start () {


			player = GameObject.FindGameObjectWithTag ("player");



		player.GetComponent<playerController>().ableToEscape = false;

		foreach (GameObject generator in GameObject.FindGameObjectsWithTag("destructible"))
		{
			if(generator.GetComponent<destructible>().getDestructibleName() == "generator"){
			generatorCount = generatorCount + 1;
			}
		}
		print ("GENERATORS : " + generatorCount);
	}
	
	// Update is called once per frame
	void Update () {

		generatorCount = 0;
		foreach (GameObject generator in GameObject.FindGameObjectsWithTag("destructible"))
		{	
			
			if(generator.GetComponent<destructible>().getDestructibleName() == "generator"){
				generatorCount = generatorCount + 1;
			}
		}
		print ("GENERATORS : " + generatorCount);


		if(generatorCount == 0)
		{
			GameObject[] forcefields = GameObject.FindGameObjectsWithTag ("forcefield");
			foreach (GameObject forcefield in forcefields) 
			{
				GameObject.Destroy(forcefield);
			}
			player.GetComponent<playerController> ().ableToEscape = true;

			player.GetComponent<playerController> ().objective = "Escape (ESC)";
		}

	}
}
