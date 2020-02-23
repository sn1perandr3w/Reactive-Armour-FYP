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

    public int shieldHealth;
    public float shieldRechargeDelay = 0.0f;


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

	public float attackCooldown = 0.0f;

	int experience;
	int level;

	float damageMultiplier = 1.0f;

	public bool guarding = false;

    public GameObject swordHitbox;
    

    int comboHit = 0;

    float comboTimer = 1.0f;

    bool lunging = false;

    float lungeTime = 0.2f;

    public GameObject pauseMenu;
    public GameObject HUD;

    public bool pausedGame = false;

    public GameObject playerCamera;


    public GameObject clusterBomb;

    public GameObject missile;
    public GameObject emptyTarget;


    public GameObject mine;

    public GameObject plasmaLanceHitbox;

    public float attackKeyDownTime = 0.0f;

    public float plasmaLanceOverheatTime = 0.0f;

    public Renderer plasmaLanceRenderer;

    public Renderer shieldRenderer;

    public GameObject sniperShell;

    int plasmaLanceMultiplier = 1;

    public bool grabHeld = false;

    public GameObject grabTarget;


    public GameObject [] clusterBombs;
    public List<Vector3> clusterBombPositions;
    public bool clusterBombsDeployed = false;

    void Start()
	{
        swordHitbox = this.gameObject.transform.GetChild(2).gameObject;
        plasmaLanceHitbox = this.gameObject.transform.GetChild(3).gameObject;

        plasmaLanceRenderer = plasmaLanceHitbox.GetComponent<Renderer>();

        shieldRenderer = this.gameObject.transform.GetChild(4).gameObject.GetComponent<Renderer>();

        clusterBombs = new GameObject[8];

        cc = this.GetComponent<CharacterController>();

        audiosources = GetComponents(typeof(AudioSource));

		audiosource = audiosources[0].GetComponent<AudioSource>();
		audiosource2 = audiosources [1].GetComponent<AudioSource>();

		audiosource2.loop = true;
		audiosource2.clip = lowHealth;

		healthLimit = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);
		health = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);
        shieldHealth = 100;
		ammoLimit = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);
		ammo = 100 + (PlayerPrefs.GetInt ("difficulty") * 25);

		combatEffectiveness = (PlayerPrefs.GetInt ("combatEffectiveness"));

		level = (PlayerPrefs.GetInt ("level"));
		experience = (PlayerPrefs.GetInt ("experience"));

		civSaved = PlayerPrefs.GetInt("civSaved");
		



		weaponSelect = 0;
		weaponList = new string[7];
		weaponList [0] = "Sword";
		weaponList [1] = "Sniper Rifle";
        weaponList [2] = "Grab";
        weaponList [3] = "Mine";
        weaponList [4] = "Missile";
        weaponList [5] = "Cluster Bomb";
        weaponList [6] = "Plasma Lance";


        clusterBombPositions.Add(transform.forward * 2.0f);
        clusterBombPositions.Add(transform.forward * -2.0f);
        clusterBombPositions.Add((transform.forward * 1.5f) + (transform.right * 1.5f));
        clusterBombPositions.Add((transform.forward * -1.5f) + (transform.right * 1.5f));
        clusterBombPositions.Add((transform.forward * 1.5f) + (transform.right * -1.5f));
        clusterBombPositions.Add((transform.forward * -1.5f) + (transform.right * -1.5f));
        clusterBombPositions.Add(transform.right * 2.0f);
        clusterBombPositions.Add(transform.right * -2.0f);

       


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

        playerCamera = GameObject.Find("PlayerCamera");
    }

	void Update()
	{
        if (pausedGame == false)
        {
            if (!cc)
            {
                print(cc);
            }

            //print ("EXPERIENCE = " + experience + " LEVEL = " + level);



            GameObject.Find("UIHealth").GetComponent<Text>().text = "Health: " + health + "%";
            GameObject.Find("UIShield").GetComponent<Text>().text = "Shield: " + shieldHealth + "%";
            GameObject.Find("UIWeapon").GetComponent<Text>().text = "Subweapon:" + weaponList[weaponSelect];
            GameObject.Find("UIAmmo").GetComponent<Text>().text = "Subweapon Ammo: " + ammo;
            GameObject.Find("UIObjective").GetComponent<Text>().text = "Objective:";
            GameObject.Find("UIObjective2").GetComponent<Text>().text = objective;
            GameObject.Find("UITotalCivs").GetComponent<Text>().text = "Total Saved: " + civSaved;
            GameObject.Find("UILevel").GetComponent<Text>().text = "Level: " + level;
            GameObject.Find("UIOverheat").GetComponent<Text>().text = (plasmaLanceOverheatTime/6.0f * 100/1).ToString("F0") + "%";
            Debug.DrawRay(transform.position + (transform.forward * 1.0f), transform.forward * 120.0f, Color.red);


            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
            }

            if (plasmaLanceOverheatTime > 0.0f && !Input.GetKey(KeyCode.Mouse0) || plasmaLanceOverheatTime > 0.0f && attackCooldown > 0.0f)
            {
                plasmaLanceOverheatTime -= Time.deltaTime;
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

            if (lunging == true)
            {
                cc.Move(transform.forward * 20.0f * Time.deltaTime);
                lungeTime -= Time.deltaTime;

                if (lungeTime <= 0.0f)
                {
                    lunging = false;
                }

            }

            if (shieldRechargeDelay <= 0.0f && shieldHealth < 100 && guarding == false)
            {
                shieldHealth++;
            }

            

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
        if (guarding == false)
        {
            health = health - amount;
            combatEffectiveness -= 25;

            if (health < 40)
            {
                audiosource2.Play();
            }


            if (health <= 0)
            {
                combatEffectiveness -= 100;
                if (combatEffectiveness < 0)
                {
                    combatEffectiveness = 0;
                }
                PlayerPrefs.SetInt("combatEffectiveness", combatEffectiveness);
                PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("gameOverScreen");

            }

            if (combatEffectiveness < 0)
            {
                combatEffectiveness = 0;
            }
            print("Player: " + gameObject.name + " health = " + health);

            
        }
        else
        {
            shieldHealth -= amount;

            if (shieldHealth < 0)
            {
                shieldHealth = 0;
            }
        }

        shieldRechargeDelay = 4.5f;
    }


	public void guard()
	{
        if (shieldHealth > 0.0)
        {
            guarding = true;
        }
        else
        {
            guarding = false;
        }
	}

	public void fireWeapon()
	{

        /*
        weaponList [0] = "Sword";
		weaponList [1] = "Sniper Rifle";
        weaponList [2] = "Grab";
        weaponList [3] = "Mine";
        weaponList [4] = "Missile";
        weaponList [5] = "Cluster Bomb";
        weaponList [6] = "Plasma Lance";
          
          
         */


        //Sword
        if (weaponSelect == 0 && attackCooldown <= 0.0f)
        {
            lunging = true;
            lungeTime = 0.2f;
            comboHit++;
            swordHitbox.GetComponent<swordHitBox>().hitboxActive(1, 2.0f, 0.25f, 25, comboHit);
            attackCooldown = 0.4f;


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
        //Sniper Rifle
        if (weaponSelect == 1 && attackCooldown <= 0.0f)
        {
            if (ammo >= 10)
            {
                if (Physics.Raycast(transform.position + (transform.forward * 1.0f), transform.forward * 120.0f, out hit))
                {
                    print("Player Hit: " + hit.transform.gameObject);
                    if (hit.transform.gameObject.tag == "destructible" && attackCooldown <= 0.0f)
                    {
                        print("SHOULD BE LOWERING HEALTH");
                        hit.transform.gameObject.GetComponent<destructible>().lowerHealth(100);
                    }
                    else
                        if (hit.transform.gameObject.tag == "enemy" && hit.transform.gameObject.GetComponent<enemyController>().guarding == false && attackCooldown <= 0.0f)
                    {
                        print("SHOULD BE LOWERING HEALTH");
                        hit.transform.gameObject.GetComponent<enemyController>().lowerHealth(Mathf.RoundToInt(50 * damageMultiplier));
                    }
                }

                Vector3 xyz = new Vector3(transform.eulerAngles.x - 90, transform.eulerAngles.y, transform.eulerAngles.z);
                Quaternion newRotation = Quaternion.Euler(xyz);

                GameObject g = (GameObject)Instantiate(sniperShell, transform.position + transform.right * 0.7f, newRotation);
                g.GetComponent<Rigidbody>().AddForce(transform.up * 250 + transform.right * 50);
                g.GetComponent<Rigidbody>().AddTorque(transform.right * -5);
                Destroy(g, 5);

                audiosource.PlayOneShot(sniperShot, 1.0f);
                attackCooldown = 2.0f;
                ammo -= 10;
            }
        }
        else
        //Grab
            if (weaponSelect == 2 && attackCooldown <= 0.0f)
        {
            //print("GRAB");

            if (grabTarget != null && grabHeld == true)
            {
                grabTarget.transform.position = transform.position + (transform.forward * 1.5f);
                grabTarget.GetComponent<enemyController>().initStun(0.5f);
                //playerCamera.GetComponent<ThirdPersonCamera>().grabCameraReset();
                //grabHeld = true;
            }
            else if (playerCamera.GetComponent<ThirdPersonCamera>().playerTarget != null && playerCamera.GetComponent<ThirdPersonCamera>().playerTarget.tag == "enemy" && grabHeld == false)
            {
                grabTarget = playerCamera.GetComponent<ThirdPersonCamera>().playerTarget;
                playerCamera.GetComponent<ThirdPersonCamera>().enemiesInLockOnRange.Remove(grabTarget);
                playerCamera.GetComponent<ThirdPersonCamera>().grabCameraReset();
                grabHeld = true;
            }
        }

        else
        //Mine
            if (weaponSelect == 3 && attackCooldown <= 0.0f)
        {
            //Vector3 minePosition = transform.position + (transform.forward * -1.0f);

            Vector3 xyz = new Vector3(transform.eulerAngles.x - 90, transform.eulerAngles.y, transform.eulerAngles.z);
            Quaternion newRotation = Quaternion.Euler(xyz);


            GameObject g = (GameObject)Instantiate(mine, (transform.position + (transform.forward * -1.0f)), newRotation);

            g.name = "Mine";


            attackCooldown = 6.0f;
        }

        else
        //Missile
            if (weaponSelect == 4 && attackCooldown <= 0.0f)
        {

            //Vector3 missilePosition = transform.position + (transform.forward * -1.0f) + (transform.right * -0.5f);

            Vector3 xyz = new Vector3(transform.eulerAngles.x - 90, transform.eulerAngles.y, transform.eulerAngles.z);
            Quaternion newRotation = Quaternion.Euler(xyz);

            for (int i = 0; i < 5; i++)
            {

                //GameObject g = (GameObject)Instantiate(missile, missilePosition, newRotation);
                //GameObject g = (GameObject)Instantiate(missile, missilePosition, Quaternion.identity);


                GameObject g = (GameObject)Instantiate(missile, (transform.position + (transform.forward * -1.0f) + (transform.right * (-0.5f + (0.25f * (i + 1))))), newRotation);

                g.name = "Missile" + (i + 1);

                g.GetComponent<MissileController>().missileNo = i;
                g.GetComponent<MissileController>().setLaunchTimer();


                if (playerCamera.GetComponent<ThirdPersonCamera>().playerTarget.tag == "enemy")
                {
                    g.GetComponent<MissileController>().target = playerCamera.GetComponent<ThirdPersonCamera>().playerTarget;
                }
                else


                if (Physics.Raycast(transform.position + (transform.forward * 1.0f), transform.forward * 120.0f, out hit))
                {
                    GameObject h = (GameObject)Instantiate(emptyTarget, hit.point, Quaternion.identity);
                    h.name = "HitTarget";
                    g.GetComponent<MissileController>().target = h;
                }

                else

                {
                    GameObject h = (GameObject)Instantiate(emptyTarget, transform.position + (transform.forward * 120.0f), Quaternion.identity);
                    h.name = "BlindTarget";
                    g.GetComponent<MissileController>().target = h;
                }


                //missilePosition = transform.position + (transform.forward * -1.0f) + (transform.right * (-0.5f + (0.25f * (i+1))));


            }

            attackCooldown = 6.0f;
        }

        else
        //Cluster Bomb
            if (weaponSelect == 5 && attackCooldown <= 0.0f && clusterBombsDeployed == false)
        {


            for (int i = 0; i < 8; i++)
            {
                GameObject g = (GameObject)Instantiate(clusterBomb, transform.position + (clusterBombPositions[i]), Quaternion.identity);
                g.name = "Cluster Bomb " + i;
                g.GetComponent<clusterBombScript>().offsetPosition = clusterBombPositions[i];
                g.GetComponent<clusterBombScript>().player = this.gameObject;

                clusterBombs[i] = g;
            }

            attackCooldown = 6.0f;



            print("CLUSTER BOMB");
            clusterBombsDeployed = true;
        }

        else
        //Plasma Lance
            if (weaponSelect == 6 && attackCooldown <= 0.0f)
        {
            plasmaLanceOverheatTime += Time.deltaTime;
            print("PLASMA LANCE");

            if (attackKeyDownTime > 2.0f)
            {
                plasmaLanceMultiplier = 2;
            }


            if (plasmaLanceOverheatTime > 6.0f)
            {
                plasmaLanceRenderer.enabled = false;
                attackCooldown = 10.0f;
                plasmaLanceHitbox.GetComponent<PlasmaLanceHitBox>().hitboxInactive();

            }
            else
            {
                plasmaLanceRenderer.enabled = true;

                plasmaLanceHitbox.GetComponent<PlasmaLanceHitBox>().hitboxActive(1, 0.5f, 10 * plasmaLanceMultiplier, 2);
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

        if (col.gameObject.tag == "OBJPickup")
        {
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
		

		

		if(Input.GetKey(KeyCode.Mouse0) && attackCooldown <= 0 && pausedGame == false) {
			//print ("ATTACKING Click");
            attackKeyDownTime += Time.deltaTime;
			fireWeapon ();
		}

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            attackKeyDownTime = 0.0f;

            if (weaponSelect == 6 && attackCooldown <= 0.0f)
            {
                plasmaLanceMultiplier = 1;
                plasmaLanceHitbox.GetComponent<PlasmaLanceHitBox>().hitboxInactive();
                plasmaLanceRenderer.enabled = false;
                attackCooldown = 2.0f;
                
            }

            if (weaponSelect == 2 && grabTarget != null)
            {
                grabTarget.transform.position = transform.position + (transform.forward * 3.0f);
                grabTarget.GetComponent<enemyController>().initKnockBack(transform, 0.6f, 80.0f);
                // playerCamera.GetComponent<ThirdPersonCamera>().enemiesInLockOnRange.Add(grabTarget);
                grabTarget.GetComponent<enemyController>().grabbed = false;
                grabTarget.GetComponent<enemyController>().thrown = true;
                grabTarget = null;
                
                grabHeld = false;
                playerCamera.GetComponent<ThirdPersonCamera>().throwCameraReset();
            }

            if (weaponSelect == 5 && clusterBombsDeployed)
            {
                for (int i = 0; i < clusterBombs.Length; i++)
                {
                    if(clusterBombs[i] != null)
                    clusterBombs[i].GetComponent<clusterBombScript>().explode();
                }

                clusterBombsDeployed = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            shieldRenderer.enabled = false;
        }

        if (Input.GetKey (KeyCode.Mouse1)) {
			print ("GUARDING");
			guard ();

            if (guarding == true)
            {
                shieldRenderer.enabled = true;
            }
            
		} else 
		{
			guarding = false;
		}

		if (Input.GetKeyDown (KeyCode.F1)) {

           

			if (Time.timeScale == 1) {
				Time.timeScale = 0;
                pausedGame = true;
                playerCamera.GetComponent<ThirdPersonCamera>().pausedGame = true;

                pauseMenu.SetActive(true);
                HUD.SetActive(false);
                //pauseMenu.GetComponent<Text>().text = "Objectives:" + "\n";
                GameObject statsText = GameObject.Find("StatsText");
                statsText.GetComponent<Text>().text = "Mech Status:" + "\n";
                statsText.GetComponent<Text>().text += "Health: " + health + "%" + "\n";
                statsText.GetComponent<Text>().text += "Subweapon Ammo: " + ammo + "%" + "\n";
                statsText.GetComponent<Text>().text += "\n" + "\n" + "Subweapons: " + "\n";

                for (int i = 0; i < weaponList.Length; i++)
                {
                    statsText.GetComponent<Text>().text += "- " + weaponList[i] + "\n";
                }


            } else 
			{
				Time.timeScale = 1;
                pausedGame = false;
                playerCamera.GetComponent<ThirdPersonCamera>().pausedGame = false;

                pauseMenu.SetActive(false);
                HUD.SetActive(true);
                
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


        if (clusterBombsDeployed == true)
        {
            for (int i = 0; i < clusterBombs.Length; i++)
            {
                if (clusterBombs[i] != null)
                    clusterBombs[i].transform.position = transform.position + clusterBombs[i].GetComponent<clusterBombScript>().offsetPosition;
            }
        }

    }
}