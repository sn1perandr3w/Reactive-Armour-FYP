using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class civController : MonoBehaviour {

	public int mapCivAmount = 0;
	public int actualCivAmount = 0;

	//test
	// Use this for initialization
	void Start () {

		foreach (GameObject civs in GameObject.FindGameObjectsWithTag("destructible"))
		{
			if (civs.GetComponent<civEntity> () != null) {
				mapCivAmount += civs.GetComponent<civEntity> ().civAmount ();
			}

		}
		actualCivAmount = mapCivAmount;

		print ("MAP CIV AMOUNT: " + mapCivAmount);
		print ("ACTUAL CIV AMOUNT: " + actualCivAmount);
	}
	
	// Update is called once per frame
	void Update () {

		GameObject.Find ("UICivilians").GetComponent<Text> ().text = "Civilians in area: " + actualCivAmount;


	}

	public void lowerCivAmount(int amount)
	{
		actualCivAmount -= amount;
	}

}
