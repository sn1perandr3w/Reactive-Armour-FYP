using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ManageWeapons : MonoBehaviour {

	//Object/value declarations

	Camera playerCamera;
	GameObject weaponOrigin;
	Ray rayFromBarrel;
	Ray rayFromCamera;
	RaycastHit hit;
	public GameObject sparksAtImpact;
	private int gunAmmo = 0;
	private const int WEAPON_GUN = 1;
	private const int WEAPON_AUTO_GUN = 0;
	private const int WEAPON_SWORD = 2;


	private float timer;
	private bool timerStarted;
	private bool canShoot = true;
	private int activeWeapon;

	private bool [] hasWeapon;
	public static int [] ammos;
	private float [] reloadTime;
	private string [] weaponNames;
	private int [] ammoCap;
	private int [] ammoPickup;
	private int [] weaponDamage;
	private int [] weaponRange;

	//Assignment of values

	void Start () {
		playerCamera = 	GetComponentInChildren<Camera>();
		weaponOrigin = transform.Find("weapon").gameObject.transform.Find("barrel").gameObject;
		ammos = new int[3];
		weaponRange = new int[4];
		hasWeapon = new bool[4];
		reloadTime = new float[4];
		weaponNames = new string[4];
		ammoCap = new int[4];
		ammoPickup = new int[4];
		weaponDamage = new int[3];


		//Active weapon
		hasWeapon [WEAPON_GUN] = true;
		hasWeapon [WEAPON_AUTO_GUN] = false;
		hasWeapon [WEAPON_SWORD] = false;
		//Damage for each weapon
		weaponDamage [WEAPON_GUN] = 50;
		weaponDamage [WEAPON_AUTO_GUN] = 20;
		weaponDamage [WEAPON_SWORD] = 75;
		//Names to display on UI
		weaponNames [WEAPON_GUN] = "Anti-Materiel Rifle";
		weaponNames [WEAPON_AUTO_GUN] = "Assault Rifle";
		weaponNames [WEAPON_SWORD] = "Energy Blade";
		//Max ammo for each weapon
		ammoCap [WEAPON_GUN] = 30;
		ammoCap [WEAPON_AUTO_GUN] = 60;
		ammoCap [WEAPON_SWORD] = 50;
		//Ammo picked up from crate per weapon type
		ammoPickup [WEAPON_GUN] = 5;
		ammoPickup [WEAPON_AUTO_GUN] = 10;
		ammoPickup [WEAPON_SWORD] = 50;
		//Ammo counts
		ammos[WEAPON_GUN] = PlayerPrefs.GetInt("gunAmmo");;
		ammos[WEAPON_AUTO_GUN] = PlayerPrefs.GetInt("autoGunAmmo");;
		ammos[WEAPON_SWORD] = PlayerPrefs.GetInt("swordAmmo");;
		//Weapon range
		weaponRange[WEAPON_GUN] = 1500;
		weaponRange[WEAPON_AUTO_GUN] = 140;
		weaponRange[WEAPON_SWORD] = 60;
		//Delay between shots
		reloadTime[WEAPON_GUN] = 1;
		reloadTime [WEAPON_AUTO_GUN] = 0;
		reloadTime [WEAPON_SWORD] = 0;
		//Weapon equipped starting off.
		activeWeapon = WEAPON_GUN;
	}
	

	void Update () {

		//Used to swap weapons.
		if (Input.GetKeyDown (KeyCode.Tab)) 
		{
			swapWeapons();
		}
		//Used to have a delay between shots.
		if (timerStarted) {
			timer += Time.deltaTime;
			if (timer >= reloadTime [activeWeapon]) 
			{
				canShoot = true;
				timerStarted = false;
			}
		}

		//UI elements
		GameObject.Find ("UIWeapon").GetComponent<Text> ().text = weaponNames[activeWeapon] + "";
		GameObject.Find ("UIAmmo").GetComponent<Text> ().text = ammos[activeWeapon] + "";
		rayFromCamera = playerCamera.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
		Debug.DrawRay (rayFromCamera.origin, rayFromCamera.direction * 2000, Color.red);


		//Raycast from Third Person Camera
		if (Physics.Raycast (rayFromCamera, out hit, weaponRange[activeWeapon]))
				{
				Vector3 positionOfImpact;
				positionOfImpact = hit.point;

				rayFromBarrel.origin = weaponOrigin.transform.position;
				rayFromBarrel.direction = -weaponOrigin.transform.position + positionOfImpact;

				Debug.DrawRay (rayFromBarrel.origin, rayFromBarrel.direction * weaponRange[activeWeapon], Color.green);

			//Raycast from the barrel of the Player's weapon. Shots come from here so as not to shoot unrealistically from the third person camera

			if (Physics.Raycast (rayFromBarrel, out hit, weaponRange[activeWeapon])) {
				Vector3 positionOfImpactActual;
				positionOfImpactActual = hit.point;

				if (Input.GetKeyDown (KeyCode.F) && ammos [activeWeapon] > 0 && canShoot) {	
					GameObject exp = (GameObject)Instantiate (sparksAtImpact, positionOfImpactActual, Quaternion.identity);
					Destroy (exp, 2);
					ammos [activeWeapon]--; 

					//Determines enemy and action.
					if (hit.collider.gameObject.tag == "enemyCQB") {
						GameObject objectTargeted = hit.collider.gameObject;
						print (objectTargeted.tag);
						objectTargeted.GetComponent<ControlCQBTurret>().lowerHealth (weaponDamage [activeWeapon]);
					}
					//Unused due to snipers being unused for this build.
					/*
					if (hit.collider.gameObject.tag == "enemySniper") {
						GameObject objectTargeted = hit.collider.gameObject;
						print (objectTargeted.tag);
						objectTargeted.GetComponent<AILauncher>().lowerHealth (weaponDamage [activeWeapon]);
					}
					*/
					canShoot = false;
					timer = 0.0f;
					timerStarted = true;
				}
				}
			}

		}

	//Used for picking up ammo.
	public void manageCollisions (Collision hit)
	{
		print ("Collided with " + hit.gameObject.name);
		if (hit.gameObject.tag == "ammo") {
			print ("Collected ammo.");
			Destroy (hit.gameObject);
			ammos[activeWeapon] += ammoPickup[activeWeapon];

			if (ammos[activeWeapon] > ammoCap[activeWeapon]) {
				ammos[activeWeapon] = ammoCap[activeWeapon];
			}


		}
	}

	//Method for weapon swapping between 3 weapons.
	public void swapWeapons()
	{
		if (activeWeapon == WEAPON_GUN) 
		{
			activeWeapon = WEAPON_AUTO_GUN;
			print ("Switched to autogun");
		}
		else if (activeWeapon == WEAPON_AUTO_GUN) 
		{
			activeWeapon = WEAPON_SWORD;
			print ("Switched to sword");
		}
		else if (activeWeapon == WEAPON_SWORD) 
		{
			activeWeapon = WEAPON_GUN;
			print ("Switched to gun");
		}
	}
}