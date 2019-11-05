using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
	public Component[] audiosources;
	AudioSource audiosource;
	AudioSource audiosource2;
	AudioSource audiosource3;

	public AudioClip swordHit;
	public AudioClip sniperShot;
	public AudioClip lowHealth;

	float inputFactor = 0.0f;
	float speedMult = 50.0f;
	CharacterController cc;
	public int healthLimit;
	int health;
	public int ammoLimit;
	int ammo;
	int civSaved;
	public bool ableToEscape;
	public string objective = "NONE";
	Ray raycast;
	RaycastHit hit;
	public LayerMask layerMask;

	private int combatEffectiveness; 

	private string [] weaponList;
	private int weaponSelect;

	float attackCooldown = 0.0f;

	int experience;
	int level;

	float damageMultiplier = 1.0f;

	public bool guarding = false;

    public GameObject swordHitbox;

    int comboHit = 0;

    float comboTimer = 1.0f;

	void Start()
	{
        swordHitbox = this.gameObject.transform.GetChild(2).gameObject;


        cc = this.GetComponent<CharacterController>();

        audiosources = GetComponents(typeof(AudioSource));

		audiosource = audiosources[0].GetComponent<AudioSource>();
		audiosource2 = audiosources [1].GetComponent<AudioSource>();

		audiosource2.loop = true;
		audiosource2.clip = lowHealth;

		healthLimit = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);
		health = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);
		ammoLimit = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);
		ammo = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);

		combatEffectiveness = (PlayerPrefs.GetInt ("combatEffectiveness"));

		level = (PlayerPrefs.GetInt ("level"));
		experience = (PlayerPrefs.GetInt ("experience"));

		civSaved = PlayerPrefs.GetInt("civSaved");
		

		weaponSelect = 0;
		weaponList = new string[2];
		weaponList [0] = "Sword";
		weaponList [1] = "Sniper Rifle";


		if (experience < 500) {
			level = 1;
		} else if (experience >+ 500 && experience < 1000) 
		{
			level = 2;
		}
		else if(experience >= 1000)
		{
			level = 3;
		}

		damageMultiplier = 1.0f * level;
	}

	void Update()
	{
        if (!cc)
        {
            print(cc);
        }

		//print ("EXPERIENCE = " + experience + " LEVEL = " + level);
		GameObject.Find ("UIHealth").GetComponent<Text> ().text = "Health: " + health + "%";
		GameObject.Find ("UIWeapon").GetComponent<Text> ().text = "Subweapon:" + weaponList[weaponSelect];
		GameObject.Find ("UIAmmo").GetComponent<Text> ().text = "Subweapon Ammo: " + ammo;
		GameObject.Find ("UIObjective").GetComponent<Text> ().text = "Objective:";
		GameObject.Find ("UIObjective2").GetComponent<Text> ().text = objective;
		GameObject.Find ("UITotalCivs").GetComponent<Text> ().text = "Total Saved: " + civSaved;
		GameObject.Find ("UILevel").GetComponent<Text> ().text = "Level: " + level;

		Debug.DrawRay (transform.position + (transform.forward * 1.0f), transform.forward * 120.0f, Color.red);


		if(attackCooldown > 0)
		{
			attackCooldown -= Time.deltaTime;
		}

        if (comboHit > 3)
        {
            comboHit = 0;
        }


        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            comboHit = 0;
        }

    }




	public void increaseAmmo( int amount)
	{
		if ((ammo + amount) < ammoLimit) {
			ammo = ammo + amount;
		} else 
		{
			ammo = ammoLimit;
		}
	}

	public void increaseCombatEffectiveness( int amount)
	{
			combatEffectiveness = combatEffectiveness + amount;
		print ("Combat Effectiveness =" + combatEffectiveness);
	}

	public void increaseExperience( int amount)
	{
		experience = experience + amount;
		print ("Experience =" + experience);

		if (experience < 500) {
			level = 1;
		} else if (experience >+ 500 && experience < 1000) 
		{
			level = 2;
		}
		else if(experience >= 1000)
		{
			level = 3;
		}

	}

	public void increaseHealth( int amount)
	{
		if ((health + amount) < healthLimit) {
			health = health + amount;
		} else 
		{
			health = healthLimit;
		}

		if(health >= 40)
		{
			audiosource2.Stop();
		}

	}

	public int getHealth()
	{
		return health;
	}

	public int getCombatEffectiveness()
	{
		return combatEffectiveness;
	}

	public int getAmmo()
	{
		return ammo;
	}

	public void lowerHealth(int amount)
	{
		if (guarding == false) {
			health = health - amount;
			combatEffectiveness -= 25;

			if (health < 40) {
				audiosource2.Play ();
			}


			if (health <= 0) {
				combatEffectiveness -= 100;
				if (combatEffectiveness < 0) {
					combatEffectiveness = 0;
				}
				PlayerPrefs.SetInt ("combatEffectiveness", combatEffectiveness);
				PlayerPrefs.SetString ("scene", SceneManager.GetActiveScene().name);
				SceneManager.LoadScene ("gameOverScreen");

			}

			if (combatEffectiveness < 0) {
				combatEffectiveness = 0;
			}
			print ("Player: " + gameObject.name + " health = " + health);
		}
	}


	public void guard()
	{
		guarding = true;
	}

	public void fireWeapon()
	{

		if (weaponSelect == 0 && attackCooldown <= 0.0f) {
            swordHitbox.GetComponent<hitBox>().hitboxActive(1,2.0f,0.25f,25);
            attackCooldown = 0.4f;
 
            comboHit++;
            comboTimer = 1.0f;
            print("COMBOHIT = " + comboHit);

            if (comboHit == 4)
            {
                print("KNOCKBACK");
            }
            /*
				if (Physics.Raycast (transform.position + (transform.forward * 1.0f), transform.forward * 10.0f, out hit)) {
					print ("Player Hit: " + hit.transform.gameObject);
				if (hit.transform.gameObject.tag == "destructible" && attackCooldown <= 0.0f) {
						print ("SHOULD BE LOWERING HEALTH");
					hit.transform.gameObject.GetComponent<destructible> ().lowerHealth (25);
					}

				if (hit.transform.gameObject.tag == "enemy" && hit.transform.gameObject.GetComponent<enemyController> ().guarding == false && attackCooldown <= 0.0f) {
						print ("SHOULD BE LOWERING HEALTH");
					hit.transform.gameObject.GetComponent<enemyController> ().lowerHealth (Mathf.RoundToInt(25 * damageMultiplier));
					}

				audiosource.PlayOneShot (swordHit, 0.4f);
				}
				attackCooldown = 0.33f;
            */
		}
        else
		if (weaponSelect == 1) {
			if (ammo >= 10) {
				if (Physics.Raycast (transform.position + (transform.forward * 1.0f), transform.forward * 120.0f, out hit)) {
					print ("Player Hit: " + hit.transform.gameObject);
					if (hit.transform.gameObject.tag == "destructible" && attackCooldown <= 0.0f) {
						print ("SHOULD BE LOWERING HEALTH");
						hit.transform.gameObject.GetComponent<destructible> ().lowerHealth (100);
					}
					else
						if (hit.transform.gameObject.tag == "enemy" && hit.transform.gameObject.GetComponent<enemyController> ().guarding == false && attackCooldown <= 0.0f) {
						print ("SHOULD BE LOWERING HEALTH");
							hit.transform.gameObject.GetComponent<enemyController> ().lowerHealth (Mathf.RoundToInt(50 * damageMultiplier));
					}
				}
				audiosource.PlayOneShot (sniperShot, 1.0f);
				attackCooldown = 2.0f;
				ammo -= 10;
			}
		}
	}

	void OnControllerColliderHit(ControllerColliderHit col)
	{
		if (col.gameObject.tag == "ammo") 
		{
			increaseAmmo(100);
			Destroy(col.gameObject);
		}

		if (col.gameObject.tag == "health") 
		{
			increaseHealth(100);
			Destroy(col.gameObject);
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (col.tag == "repairStation")
		{
			increaseHealth (1);
		}
	}


	void LateUpdate()	
	{

		if (Input.GetKey (KeyCode.LeftShift)) 
		{
			speedMult = 100.0f;
		} else 
		{
			speedMult = 50.0f;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * speedMult;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * speedMult;
		

		

		if(Input.GetKey(KeyCode.Mouse0) && attackCooldown <= 0) {
			//print ("ATTACKING");
			fireWeapon ();
		}

		if (Input.GetKey (KeyCode.Mouse1)) {
			print ("GUARDING");
			guard ();
		} else 
		{
			guarding = false;
		}

		if (Input.GetKeyDown (KeyCode.F1)) {

			if (Time.timeScale == 1) {
				Time.timeScale = 0;
			} else 
			{
				Time.timeScale = 1;
			}
		} 

		if (Input.GetKeyDown (KeyCode.Tab)) {

			if (weaponSelect < weaponList.Length -1) {
				weaponSelect++;
			} else 
			{
				weaponSelect = 0;
			}
		} 

		if (Input.GetKey (KeyCode.Escape)) {
			if (ableToEscape == true) {
				GameObject civController = GameObject.Find("civController");
				civSaved += civController.GetComponent<civController> ().actualCivAmount;
				PlayerPrefs.SetInt ("civSaved",civSaved);
				PlayerPrefs.SetInt ("combatEffectiveness",combatEffectiveness);
				PlayerPrefs.SetInt ("level",level);
				PlayerPrefs.SetInt ("experience",experience);
				PlayerPrefs.SetString ("scene",SceneManager.GetActiveScene().name);
				SceneManager.LoadScene ("mapScreen");
			} else 
			{
				print ("Cannot escape!");
			}
		}

		if(Input.GetKey(KeyCode.Space)) {
			//print("VERTICAL MOVEMENT");
			inputFactor+= 0.05f;
		}
		else
		if(Input.GetKey(KeyCode.LeftControl)) {
			inputFactor-= 0.05f;
		}
		else
		{
			if(inputFactor <= -0.05f)
			{
				inputFactor += 0.05f;
			}
			else if(inputFactor >= 0.05f)
			{
				inputFactor -= 0.05f;
			}
			else
			inputFactor = 0.0f;
		}
		inputFactor = Mathf.Clamp(inputFactor,-1.0f,1.0f	);
		var y = (inputFactor * Time.deltaTime * speedMult);
		

		//print("Input Factor: " + inputFactor);

		Vector3 movement = new Vector3 (x,y,z);


		cc.Move (transform.TransformDirection(movement));


	}
}