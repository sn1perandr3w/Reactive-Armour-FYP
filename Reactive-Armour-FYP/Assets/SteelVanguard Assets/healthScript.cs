using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This is the main script controlling player interaction in regards to damage as well as collision with objects.

public class healthScript : MonoBehaviour {

	//Declaration of variables.


	public static int powerCellMax = 2;
	public static int powerCellSpawns = 0;
	public  static int powerCells = 0;
	public static bool squadFire = true;
	public static int cellsPlaced = 0;
	public int health = 100;
	float time;


	// Use this for initialization
	void Start () {

		//Sets variables which have already been declared to values from playerPrefs.

		powerCells = PlayerPrefs.GetInt("powerCells");
		health = PlayerPrefs.GetInt("health");
		time = PlayerPrefs.GetFloat("time");
	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		GameObject.Find ("UITime").GetComponent<Text> ().text = time + "";
		GameObject.Find ("UITime2").GetComponent<Text> ().text = "Seconds Passed";
		GameObject.Find ("UIHealth").GetComponent<Text> ().text = health + "";
		GameObject.Find ("UIPowerCells2").GetComponent<Text> ().text = powerCells + "";

		print (health + " Health");
		print (powerCells + " Power cells in inventory.");

		//Causes Game Over upon losing all health.

		if (health < 1) {
			SceneManager.LoadScene ("GameOver");
		}

		if (health > 100) 
		{
			health = 100;
		}

	}

	//Used to interact with objects based on collision.

	void OnCollisionEnter(Collision collision)
	{

		//Causes victory and resets playerPrefs should the player want to play again.
		if (cellsPlaced > 3) 
		{
			squadFire = false;
			SceneManager.LoadScene ("Victory");
			cellsPlaced = 0;
			PlayerPrefs.SetInt ("health", 100);
			PlayerPrefs.SetInt ("powerCells", 0);


		}


		//Used for selecting objects that have been collided with.
		string name = collision.collider.gameObject.name;
		string tagName = collision.collider.gameObject.tag;

		//Kills player if touching water.
		if(tagName == "water")
		{
			lowerHealth(100);
		}
		//Picks up powercell.
		if(tagName == "powercell")
		{
			addCell(1);
			Object.Destroy (collision.collider.gameObject);
		}
		//Changes scene depending on door touched.
		if(tagName == "openDoor")
		{
			powerCellSpawns = 0;
			PlayerPrefs.SetInt("spawnLocation",1);
			squadFire = false;
			SceneManager.LoadScene("Steel Vanguard Scene 2");
			PlayerPrefs.SetInt ("health", health);
			PlayerPrefs.SetInt ("powerCells", powerCells);
			PlayerPrefs.SetFloat ("time", time);
			PlayerPrefs.SetInt("gunAmmo", ManageWeapons.ammos[1]);
			PlayerPrefs.SetInt("autoGunAmmo", ManageWeapons.ammos[0]);
			PlayerPrefs.SetInt("swordAmmo", ManageWeapons.ammos[2]);
		}
		//Changes scene depending on door touched.
		if(tagName == "openDoor2")
		{
			powerCellSpawns = 0;
			squadFire = false;
			SceneManager.LoadScene("Steel Vanguard Scene 1");
			PlayerPrefs.SetInt ("health", health);
			PlayerPrefs.SetInt ("powerCells", powerCells);
			PlayerPrefs.SetFloat ("time", time);
			PlayerPrefs.SetInt("gunAmmo", ManageWeapons.ammos[1]);
			PlayerPrefs.SetInt("autoGunAmmo", ManageWeapons.ammos[0]);
			PlayerPrefs.SetInt("swordAmmo", ManageWeapons.ammos[2]);
		}

		if(tagName == "openDoor3")
		{
			powerCellSpawns = 0;
			PlayerPrefs.SetInt("spawnLocation",2);
			squadFire = false;
			SceneManager.LoadScene("Steel Vanguard Scene 3");
			PlayerPrefs.SetInt ("health", health);
			PlayerPrefs.SetInt ("powerCells", powerCells);
			PlayerPrefs.SetFloat ("time", time);
			PlayerPrefs.SetInt("gunAmmo", ManageWeapons.ammos[1]);
			PlayerPrefs.SetInt("autoGunAmmo", ManageWeapons.ammos[0]);
			PlayerPrefs.SetInt("swordAmmo", ManageWeapons.ammos[2]);
		}

		if(tagName == "openDoor4")
		{
			powerCellSpawns = 0;
			squadFire = false;
			SceneManager.LoadScene("Steel Vanguard Scene 1");
			PlayerPrefs.SetInt ("health", health);
			PlayerPrefs.SetInt ("powerCells", powerCells);
			PlayerPrefs.SetFloat ("time", time);
			PlayerPrefs.SetInt("gunAmmo", ManageWeapons.ammos[1]);
			PlayerPrefs.SetInt("autoGunAmmo", ManageWeapons.ammos[0]);
			PlayerPrefs.SetInt("swordAmmo", ManageWeapons.ammos[2]);
		}

		//Heals player.
		if(tagName == "healPad")
		{
			addHealth(100);
		}

		//Sets down power cell into plug, removing it from inventory.
		if (tagName == "powerPlug") {
			if (powerCells > 0) {
				Object.Destroy (collision.collider.gameObject);
				cellsPlaced = cellsPlaced + 1;
				powerCells = powerCells - 1;
			}
		}



	}

	//Adds health to player.
	public void addHealth(int addition)
	{
		health = health + addition;
		print ("Healed " + addition + " health.");
	}
	//Removes health from player.
	public void lowerHealth(int removal)
	{
		health = health - removal;
		print ("Took " + removal + " damage.");
	}
	//Adds power cell to player inventory.
	public void addCell(int addition)
	{
		powerCells = powerCells + addition;
		print (addition + " cells added to inventory.");
	}

}