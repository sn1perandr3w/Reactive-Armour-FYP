using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

	public Transform enemyTarget;
	public Transform target;
	public Transform targetStored;
	public int healthLimit;
	public int health;

	public GameObject explosionSound;

	public string enemyName;

	float rotationalDamp = 60.0f;

	float movementSpeed = 4.0f;
	public CharacterController cc;

	public List<GameObject> civilians;

	public GameObject currentHitObject;

	private float currentHitDistance;

	public float sphereRadius;
	public float maxDistance = 25.0f;
	public LayerMask layerMask;

	private Vector3 origin;
	private Vector3 direction;

	public List<Transform> tempWayPoints;

	private Vector3 dir1;
	private Vector3 dir2;
	private Vector3 dir3;
	private Vector3 dir4;

	public bool maneuvering = false;

	public GameObject tempWayPoint;
	public GameObject lastKnownPosWayPoint;
	Animator anim;
	AnimatorStateInfo info;

	//Stunning
	float stunTime = 0.0f;
	//Guarding
	float guardTime = 0.0f;
	public bool guarding = false;

	float pursueBuffer = 0.0f;

	public GameObject[] patrolWaypoints;
	int waypointNum = 0;


	public float attackCooldown = 0.0f;

	//Time until returning to patrol
	float timeUntilPatrol = 0;

	public GameObject player;
	float distanceToPlayer;

	public int selection;
	float strafeTimer;
	int strafeDir;

	public GameObject projectile;

	public GameObject[] itemDrop;



    public Transform receivedHit;
    public float knockbackTime;
    public float knockbackForce;

    public string triggerForReturn;

    public bool grabbed = false;

    public bool thrown = false;

    int strafeDirFB;
    int strafeDirLR;

    void Start ()
	{
		anim = GetComponent<Animator> ();
		cc = GetComponent<CharacterController> ();

			player = GameObject.FindGameObjectWithTag ("player");


		if (target != null && target.transform.tag == "player") {
			anim.SetTrigger ("pursue");
		} else if (patrolWaypoints.Length != 0) {
			target = patrolWaypoints [0].transform;
			anim.SetTrigger ("patrol");
		}

		selection = Random.Range (1, 4);

		

		foreach (GameObject civTarget in GameObject.FindGameObjectsWithTag("destructible")) {
			if (civTarget.GetComponent<civEntity> () != null) {
				civilians.Add (civTarget);
			}
		}

        foreach (GameObject civTarget in GameObject.FindGameObjectsWithTag("ally"))
        {
            if (civTarget.GetComponent<civEntity>() != null)
            {
                civilians.Add(civTarget);
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
	{
		
		distanceToPlayer = Vector3.Distance (transform.position, player.transform.position);

		info = anim.GetCurrentAnimatorStateInfo (0);

		Debug.DrawRay (transform.position, transform.forward * 3.0f, Color.blue);




		if (distanceToPlayer < 100.0f && pursueBuffer <= 0.0f && (info.IsName("Patrolling") || info.IsName("Searching") || info.IsName("CombatCiv"))) {
			
				pursueBuffer = 2.0f;
				
				target = player.transform;

            anim.SetTrigger("pursue");


		}

		if (attackCooldown > 0) {
			attackCooldown -= Time.deltaTime;
		}


        if (strafeTimer > 0)
        {
            strafeTimer -= Time.deltaTime;
        }


        if (info.IsName("Pursuing"))
        {
            //print ("PURSUING");
            //CollisionAvoidance();
            Turn();
            Move();
        }
        else if (info.IsName("Patrolling"))
        {
            //print ("PATROLLING");
            CollisionAvoidance();
            CheckForCivTargets();
            Move();

        }
        else if (info.IsName("Searching"))
        {
            //print ("SEARCHING");
            LowerPursueBuffer();
            //CollisionAvoidance ();
            Move();
            Turn();
        }
        else if (info.IsName("Combat"))
        {
            //print ("COMBATTING");
            combatMove();
        }
        else if (info.IsName("MeleeAttack"))
        {
            //print ("MELEE ATTACKING");
            meleeAttack();
        }
        else if (info.IsName("RangedAttack"))
        {
            //print ("RANGED ATTACKING");
            rangedAttack();
        }
        else if (info.IsName("Guard"))
        {
            //print ("GUARDING");
            guard();
        }
        else if (info.IsName("CombatCiv"))
        {
            //print ("CombattingCiv");
            Turn();
        }
        else if (info.IsName("Stunned"))
        {
            if (stunTime > 0.0f)
            {
                stunTime -= Time.deltaTime;
            }
            else
            {
                anim.SetTrigger(triggerForReturn);
            }
        }
        else if (info.IsName("Knockback"))
        {

            if (knockbackTime > 0.0f)
            {
                cc.Move(-transform.forward * knockbackForce * Time.deltaTime);
                knockbackTime -= Time.deltaTime;
                //print("KNOCKBACK TIME = " + timeUntilKnockbackEnds);
                //knockbackForce = knockbackForce * knockbackTime;

                

            }
            else
            {
                thrown = false;
                anim.SetTrigger(triggerForReturn);
            }


        }
        
	}

	//Creates a melee attack using a raycast

	public void meleeAttack ()
	{
        //print(this.name + " Melee Attack");

        if (attackCooldown <= 0.0f) {
			//Melee hit
			RaycastHit matkHit;

            

			if (Physics.Raycast (transform.position, transform.forward, out matkHit, 12.0f)) {
				//print ("Enemy Hit: " + matkHit.transform.gameObject);
				if (matkHit.transform.gameObject.tag == "player") {
					//print ("SHOULD BE LOWERING HEALTH");
					matkHit.transform.gameObject.GetComponent<playerController> ().lowerHealth (20);
				}
			}
            attackCooldown = 4.0f;
        }
		
        //newSelection ();
        anim.SetTrigger("combat");
        
	}

	//Creates a ranged attack using a raycast.

	public void rangedAttack ()
	{
        //print(this.name + " Ranged Attack");

       if (attackCooldown <= 0.0f) {
			//print ("RANGED ATTACK!");
			
			GameObject g = (GameObject)Instantiate (projectile, transform.position + transform.forward * 3.0f, Quaternion.identity);	
			g.GetComponent<Rigidbody> ().AddForce (transform.forward * 4000);
			Destroy (g, 1);
            attackCooldown = 6.0f;
        }
        //newSelection();
        anim.SetTrigger(triggerForReturn);

	}

	//Avoids obstacles and calls Turn if not doing so.

	void CollisionAvoidance ()
	{
		tempWayPoints.Clear ();
		currentHitDistance = maxDistance;
		origin = transform.position + (transform.forward * 1f);
		direction = transform.forward;
		
		dir1 = ((transform.up * 0.5f) + transform.forward);
		dir2 = ((transform.right * 0.5f) + transform.forward);
		dir3 = ((transform.up * -0.5f) + transform.forward);
		dir4 = ((transform.right * -0.5f) + transform.forward);

		RaycastHit hit;

		if (maneuvering == false) {

			if (Physics.SphereCast (origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal)) {
				currentHitObject = hit.transform.gameObject;
				currentHitDistance = hit.distance;
				//print (hit.transform.gameObject.name);

				if (currentHitObject.tag != "player") {
					//TESTING UP
					if (Physics.SphereCast (origin, sphereRadius, dir1, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal)) {
						//print ("CAN'T GO UP");
					} else {
						print ("CREATING WAYPOINT UP");
						GameObject WayPointUp = GameObject.Instantiate (tempWayPoint, origin + dir1 * maxDistance, Quaternion.identity);
						tempWayPoints.Add (WayPointUp.transform);
					}
					//TESTING RIGHT
					if (Physics.SphereCast (origin, sphereRadius, dir2, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal)) {
						//print ("CAN'T GO RIGHT");
					} else {
						//print ("CREATING WAYPOINT RIGHT");
						GameObject WayPointRight = GameObject.Instantiate (tempWayPoint, origin + dir2 * maxDistance, Quaternion.identity);
						tempWayPoints.Add (WayPointRight.transform);
					}
					//TESTING DOWN
					if (Physics.SphereCast (origin, sphereRadius, dir3, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal)) {
						//print ("CAN'T GO DOWN");
					} else {
						//print ("CREATING WAYPOINT DOWN");
						GameObject WayPointDown = GameObject.Instantiate (tempWayPoint, origin + dir3 * maxDistance, Quaternion.identity);
						tempWayPoints.Add (WayPointDown.transform);
					}
					//TESTING LEFT
					if (Physics.SphereCast (origin, sphereRadius, dir4, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal)) {
						//print ("CAN'T GO LEFT");
					} else {
						//print ("CREATING WAYPOINT LEFT");
						GameObject WayPointLeft = GameObject.Instantiate (tempWayPoint, origin + dir4 * maxDistance, Quaternion.identity);
						tempWayPoints.Add (WayPointLeft.transform);
					}
			
				}
			}
		}

		if (tempWayPoints.Count == 0) {
			Turn ();
		} else {
			targetStored = target;
			Transform tempWayPointToFollow = tempWayPoints [Random.Range (0, tempWayPoints.Count - 1)].transform;
			target = tempWayPointToFollow;
			maneuvering = true;
		}
	}


	void CheckForCivTargets()
	{
        
            foreach (GameObject civTarget in civilians)
            {
                if (civTarget != null)
                {
                    float enemyDistanceToCivTarget = Vector3.Distance(this.transform.position, civTarget.transform.position);
                    if (enemyDistanceToCivTarget <= 200.0f)
                    {
                        target = civTarget.transform;
                        anim.SetTrigger("combatCiv");
                    }
                }
                else if (civTarget == null)
                {
                    civilians.Remove(civTarget);
                }
            }
        
	}


	//Advances NPC towards target.

	void Move()
	{
        //print("Moving");
		cc.Move (transform.forward * movementSpeed * Time.deltaTime);
	}

    public void initKnockBack(Transform receivedHit, float knockbackTime, float knockbackForce)
    {
        this.knockbackTime = knockbackTime;
        this.knockbackForce = knockbackForce;

        Vector3 pos = receivedHit.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        anim.SetTrigger("knockback");
        print("KNOCKBACK SET");
        

        if (info.IsName("Pursuing"))
        {
            triggerForReturn = "pursue";
        }
        else
        {
            triggerForReturn = "combat";
        }
    }


    //Causes strafing when in combat.

    void combatStrafe ()
	{
        /*
		//print ("DIRECTION SELECTED! " + strafeDir);
		//print ("STRAFE TIMER: " + strafeTimer);
		if (strafeTimer > 0) {

			if (strafeDir == 1) {
				cc.Move (transform.right * movementSpeed * Time.deltaTime);
			} else if (strafeDir == 2) {
				cc.Move (-transform.right * movementSpeed * Time.deltaTime);
			}
		} else {
			strafeDir = Random.Range (1, 3);
			strafeTimer = Random.Range (1, 4);

		}

		strafeTimer -= Time.deltaTime;

        */

        if (strafeDirFB == 1 && distanceToPlayer > 2.0f)
        {
            cc.Move(transform.forward * movementSpeed * Time.deltaTime);
        }
        else if (strafeDirFB == 2 && distanceToPlayer < 10.0f)
        {
            cc.Move(-transform.forward * movementSpeed * Time.deltaTime);
        }

        if (strafeDirLR == 1)
        {
            cc.Move(transform.right * movementSpeed * Time.deltaTime);
        }
        else if (strafeDirLR == 2)
        {
            cc.Move(-transform.right * movementSpeed * Time.deltaTime);
        }


    }

	//Faces NPC towards target, be it player or waypoint. Also used to make decisions based on distance to target.

	void Turn ()
	{
		//print ("Target = " + target.name);

		if ((target == null || Vector3.Distance(transform.position, target.transform.position) > 50) && info.IsName("CombatCiv")) 
		{
			target = patrolWaypoints [waypointNum].transform;
            anim.SetTrigger("patrol");
        }


        //Vector3 pos = (target.position + (transform.up * 1)) - transform.position;
        Vector3 pos = (target.position) - transform.position;


        Quaternion rotation = Quaternion.LookRotation (pos);

		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationalDamp * Time.deltaTime);

		//print ("TARGET = " + target);
		//print ("TARGET TRANSFORM TAG = " + target.transform.tag);

		float distance = Vector3.Distance (transform.position, target.position);

		if (info.IsName ("Pursuing")) {
			if (target.transform.tag == "player" && distance >= 100.0f) {
				print ("LOST TARGET. SEARCHING LAST KNOWN POSITION");
				GameObject lastKnownPosWP = GameObject.Instantiate (lastKnownPosWayPoint, target.position, Quaternion.identity);
				target = lastKnownPosWP.transform;

                anim.SetTrigger("search");
            } else if (distance < 20 && target.transform.tag == "player") {

                anim.SetTrigger("combat"); ;
			}
		}
        else
		if (info.IsName ("CombatCiv")) {


			if (distance >= 40.0f) {
				Move ();
			} else
			{
                triggerForReturn = "combatCiv";
                anim.SetTrigger("ranged");
            }
		}
        else
		if (info.IsName ("Searching") && target.tag == "lastKnownWayPoint") {
			if (distance > 3.0f) {
				Move ();
			}

			print ("SEARCHING FOR WAYPOINT. DISTANCE TO WAYPOINT = " + distance);
			if (distance <= 3.0f) {
				timeUntilPatrol += Time.deltaTime;
				print ("REACHED LAST KNOWN POSITION");
				if (timeUntilPatrol >= 10.0f) {
					target = patrolWaypoints [waypointNum].transform;
					distance = Vector3.Distance (transform.position, target.position);

                    anim.SetTrigger("patrol");
                    timeUntilPatrol = 0.0f;
				}
			}
		}
			
		if (target.transform.tag == "tempWayPoint") {
				
			if (distance < 3.0f) {
				target = targetStored;
				maneuvering = false;
			}
		} else if (target.transform.tag == "wayPoint") {

			if (distance < 3.0f) {
				if (waypointNum == patrolWaypoints.Length - 1) {
					waypointNum = 0;
				} else {
					waypointNum++;
				}
			}
			target = patrolWaypoints [waypointNum].transform;
		}
	}

	//Prevents from performing actions

	public void initStun (float stunTime)
	{
        this.stunTime = stunTime;

        if (info.IsName("Pursuing"))
        {
            triggerForReturn = "pursue";
        }
        else
        {
            triggerForReturn = "combat";
        }
        anim.SetTrigger("stun");
    }

	//Prevents from taking damage

	public void guard ()
	{
		while (guardTime < 1) {
			guardTime += Time.deltaTime;
			guarding = true;
		}

       
        guardTime = 0;
		attackCooldown = 6.0f;
        anim.SetTrigger("combat");
        //newSelection ();
    }

    //Moving once in combat range



    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print("Collided with " + hit.gameObject.name);

        if (thrown == true && hit.gameObject.tag != "player")
        {
            if (hit.gameObject.tag == "enemy")
            {
                if (hit.gameObject.GetComponent<enemyController>() != null)
                {
                    hit.gameObject.GetComponent<enemyController>().initStun(2.0f);
                    hit.gameObject.GetComponent<enemyController>().lowerHealth(50);
                }
                else if (hit.gameObject.GetComponent<EnemySniperController>() != null)
                {
                    hit.gameObject.GetComponent<EnemySniperController>().initStun(2.0f);
                    hit.gameObject.GetComponent<EnemySniperController>().lowerHealth(50);
                }
                if (hit.gameObject.GetComponent<EnemyMedicController>() != null)
                {
                    hit.gameObject.GetComponent<EnemyMedicController>().initStun(2.0f);
                    hit.gameObject.GetComponent<EnemyMedicController>().lowerHealth(50);
                }
            }

            else if (hit.gameObject.tag == "destructible")
            {
                hit.gameObject.GetComponent<destructible>().lowerHealth(50);
            }

            
            anim.SetTrigger(triggerForReturn);
            lowerHealth(50);
            thrown = false;
        }
    }


    public void combatMove ()
	{
		Turn ();

        /*
        print("Distance To Player: " + distanceToPlayer);

		if (distanceToPlayer > 20.0f && distanceToPlayer < 100.0f) {
			Move ();
		} else if (distanceToPlayer <= 20.0f) {
			combatStrafe ();
		}

        
		//print ("SELECTION = " + selection);
		if (attackCooldown <= 0.0f) {
			if (selection == 1 && distanceToPlayer <= 6.0f) {

                anim.SetTrigger("melee");
            } else if (selection == 2) {
                triggerForReturn = "combat";
                anim.SetTrigger("ranged");
            } else if (selection == 3) {

                anim.SetTrigger("guard");
            }
		}*/
        /*
        if (attackCooldown <= 0.0f)
        {
            newSelection();
        }
        */
        if (strafeTimer <= 0.0f)
        {
            strafeDirFB = Random.Range(0, 3);
            strafeDirLR = Random.Range(0, 3);
            strafeTimer = Random.Range(0.5f, 1.5f);
        }
        else if (attackCooldown <= 0.0f)
        {
            if (selection == 1 && distanceToPlayer <= 10.0f)
            {
                triggerForReturn = "combat";
                anim.SetTrigger("melee");
            }
            else if (selection == 2 && distanceToPlayer > 10.0f || selection == 2)
            {
                triggerForReturn = "combat";
                anim.SetTrigger("ranged");
                
            }
            else if (selection == 3)
            {
                triggerForReturn = "combat";
                anim.SetTrigger("guard");
            }
        }
        else
        {
            combatStrafe();
        }



	}

	//Creates a selection of which action to perform in combat

	public void newSelection ()
	{
		selection = Random.Range (1, 4);
	}

	//Used for not instantly falling back into pursue when in searching if player skims the boundary of the enemy line of sight.

	public void LowerPursueBuffer ()
	{
		pursueBuffer -= Time.deltaTime;
	}

	//Increases health

	public void increaseHealth (int amount)
	{
		if ((health + amount) < healthLimit) {
			health = health + amount;
		} else {
			health = healthLimit;
		}
	}

	//Lowers health

	public void lowerHealth (int amount)
	{
		health = health - amount;
		if (health <= 0) {
			int dropSel = Random.Range(1, 100);
			int itemSel = Random.Range(0,2);

            print("DropSel: " + dropSel + " ItemSel: " + itemSel);


			print ("DROPSEL: " + dropSel + " ITEMSEL: " + itemSel);
			if (itemSel == 0) {
				if (player.GetComponent<playerController> ().getHealth () < 50) {
					dropSel += 40;
				}
			}
            else
			if (itemSel == 1) {
				if (player.GetComponent<playerController> ().getAmmo () < 50) {
					dropSel += 40;
				}
			}
            
			if(dropSel >= 80)
			{
				GameObject g = (GameObject)Instantiate (itemDrop[itemSel], transform.position, Quaternion.identity);
			}

			GameObject e = (GameObject)Instantiate(explosionSound, transform.position, Quaternion.identity);
			//player.GetComponent<playerController> ().increaseExperience (100);
			//player.GetComponent<playerController> ().increaseCombatEffectiveness (100);

			Destroy (this.gameObject);
		}


		print ("Enemy: " + gameObject.name  + " Lowered health by " + amount + " health = " + health);
	}

}
