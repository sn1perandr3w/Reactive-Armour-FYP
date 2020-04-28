using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatEngineerController : MonoBehaviour
{

    public Transform enemyTarget;
    public Transform target;
    public Transform targetStored;
    public int healthLimit;
    public int health;

    public GameObject explosionSound;

    public string enemyName;

    float rotationalDamp = 60.0f;

    float movementSpeed = 40.0f;
    public CharacterController cc;


    public GameObject currentHitObject;

    private float currentHitDistance;

    public float sphereRadius;
    public float maxDistance = 25.0f;
    public LayerMask layerMask;

    private Vector3 origin;
    private Vector3 direction;


    Animator anim;
    AnimatorStateInfo info;

    //Stunning
    float stunTime = 0.0f;


    public float pursueBuffer = 0.0f;

    public GameObject[] evadeWaypoints;
    public int waypointNum = 0;


    public float attackCooldown = 0.0f;

    public float mineCooldown = 0.0f;

    public GameObject player;
    float distanceToPlayer;

    int selection;

    public GameObject heavyProjectile;
    public GameObject lightProjectile;

    public GameObject[] itemDrop;



    public Transform receivedHit;
    public float knockbackTime;
    public float knockbackForce;

    public string triggerForReturn;

    public bool grabbed = false;

    public bool thrown = false;

    public bool isTargeted = false;

    public float timeUntilEvade = 0.0f;

    public GameObject mine;


    public float burstTime = 0.0f;

    public AudioSource lightShotSource;
    public AudioSource heavyShotSource;

    public List<GameObject> turrets;


    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        player = GameObject.FindGameObjectWithTag("player");



    }

    // Update is called once per frame
    void Update()
    {

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        info = anim.GetCurrentAnimatorStateInfo(0);

        Debug.DrawRay(transform.position, transform.forward * 3.0f, Color.blue);

        //print("Target = " + target);

        if (pursueBuffer >= 0.0f)
        {
            pursueBuffer -= Time.deltaTime;
        }

        if (distanceToPlayer > 30.0f && distanceToPlayer < 120.0f && pursueBuffer <= 0.0f && info.IsName("Idle"))
        {

            pursueBuffer = 2.0f;

            target = player.transform;
            print("Target = " + target);

            anim.SetTrigger("combat");


        }


        if (attackCooldown > 0.0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (mineCooldown > 0.0f)
        {
            mineCooldown -= Time.deltaTime;
        }

        if (burstTime > 0.0f)
        {
            burstTime -= Time.deltaTime;
        }

        if (timeUntilEvade > 0.0f)
        {
            timeUntilEvade -= Time.deltaTime;
        }



        if (info.IsName("Evading"))
        {
            if (timeUntilEvade <= 0.0f)
            {
                Turn();
                Move();
            }
        }

        else if (info.IsName("Combat"))
        {
            Turn();
            if (attackCooldown <= 0.0f && distanceToPlayer > 30.0f)
            {
                triggerForReturn = "combat";
                anim.ResetTrigger("combat");

                int rangedSel = Random.Range(0,2);

                if (rangedSel == 0)
                {
                    burstTime = 5.0f;
                    anim.SetTrigger("lightranged");
                }
                else
                {
                    anim.SetTrigger("heavyranged");
                }
            }
            else if (distanceToPlayer <= 30.0f)
            {
                
                timeUntilEvade = 5.0f;
                anim.SetTrigger("evade");
                
            }
        }

        else if (info.IsName("LightRangedAttack"))
        {
            //print ("RANGED ATTACKING");
            lightRangedAttack();
        }
        else if (info.IsName("HeavyRangedAttack"))
        {
            //print ("RANGED ATTACKING");
            heavyRangedAttack();
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


    //Creates a ranged attack using a raycast.

    public void heavyRangedAttack()
    {
        if (attackCooldown <= 0.0f)
        {

            GameObject g = (GameObject)Instantiate(heavyProjectile, transform.position + transform.forward * 5.0f, Quaternion.identity);
            g.GetComponent<Rigidbody>().AddForce(transform.forward * 4000);
            Destroy(g, 15);
            attackCooldown = 2.0f;
            heavyShotSource.Play();
        }
        //newSelection();
        anim.ResetTrigger("heavyranged");
        anim.SetTrigger(triggerForReturn);

    }

    public void lightRangedAttack()
    {
        Turn();

        if (burstTime > 0.0f)
        {
            if (attackCooldown <= 0.0f)
            {

                GameObject g = (GameObject)Instantiate(lightProjectile, transform.position + transform.forward * 5.0f, Quaternion.identity);
                g.GetComponent<Rigidbody>().AddForce(transform.forward * 4000);
                Destroy(g, 15);
                attackCooldown = 0.2f;
                lightShotSource.Play();
            }
        }
        else
        {
            attackCooldown = 4.0f;
            //newSelection();
            anim.ResetTrigger("lightranged");
            anim.SetTrigger(triggerForReturn);
        }
    }

    //Advances NPC towards target.

    void Move()
    {
        //print("Moving");
        cc.Move(transform.forward * movementSpeed * Time.deltaTime);
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


        if (info.IsName("Idle"))
        {
            triggerForReturn = "idle";
        }
        else
        {
            triggerForReturn = "combat";
        }
    }



    //Faces NPC towards target, be it player or waypoint. Also used to make decisions based on distance to target.

    void Turn()
    {
        if (info.IsName("Evading"))
        {
            target = evadeWaypoints[waypointNum].transform;
        }

        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);

        //print ("TARGET = " + target);
        //print ("TARGET TRANSFORM TAG = " + target.transform.tag);

        float distance = Vector3.Distance(transform.position, target.position);

        if (target.transform.tag == "wayPoint" && info.IsName("Evading"))
        {
            if (distance > 35.0f)
            {
                if (mineCooldown <= 0.0f)
                {
                    dropMine();
                }
            }


            if (distance < 3.0f)
            {
                if (waypointNum == evadeWaypoints.Length - 1)
                {
                    waypointNum = 0;
                }
                else
                {
                    waypointNum++;
                }
                //target = null;

                

                anim.ResetTrigger("evade");
                anim.ResetTrigger("combat");
                anim.SetTrigger("idle");
                
            }


        }

    }

    //Prevents from performing actions

    public void initStun(float stunTime)
    {
        this.stunTime = stunTime;

        if (info.IsName("Idle"))
        {
            triggerForReturn = "idle";
        }
        else
        {
            triggerForReturn = "combat";
        }
        anim.SetTrigger("stun");
    }



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

    public void dropMine()
    {
        Vector3 xyz = new Vector3(transform.eulerAngles.x - 90, transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(xyz);


        GameObject g = (GameObject)Instantiate(mine, (transform.position + (transform.forward * -5.0f)), newRotation);

        g.name = "EnemyMine";

        mineCooldown = 2.0f;
    }

    //Increases health

    public void increaseHealth(int amount)
    {
        if ((health + amount) < healthLimit)
        {
            health = health + amount;
        }
        else
        {
            health = healthLimit;
        }
    }


    //Lowers health

    public void lowerHealth(int amount)
    {
        health = health - amount;
        if (health <= 0)
        {
            int dropSel = Random.Range(1, 100);
            int itemSel = Random.Range(0, 2);

            print("DROPSEL: " + dropSel + " ITEMSEL: " + itemSel);
            if (itemSel == 0)
            {
                if (player.GetComponent<playerController>().getHealth() < 50)
                {
                    dropSel += 40;
                }
            }
            else
            if (itemSel == 1)
            {
                if (player.GetComponent<playerController>().getAmmo() < 50)
                {
                    dropSel += 40;
                }
            }
            else
            if (dropSel >= 80)
            {
                GameObject g = (GameObject)Instantiate(itemDrop[itemSel], transform.position, Quaternion.identity);
            }

            GameObject e = (GameObject)Instantiate(explosionSound, transform.position, Quaternion.identity);
            player.GetComponent<playerController>().increaseExperience(100);
            player.GetComponent<playerController>().increaseCombatEffectiveness(100);

            Destroy(this.gameObject);
        }

        else if (health < healthLimit && turrets[0].GetComponent<EnemyTurretController>().active == false)
        {
            turrets[0].GetComponent<EnemyTurretController>().active = true;
        }
        else if (health < healthLimit - (healthLimit / 3) && turrets[1].GetComponent<EnemyTurretController>().active == false)
        {
            turrets[1].GetComponent<EnemyTurretController>().active = true;
        }
        else if (health < healthLimit - (healthLimit / 3) * 2 && turrets[2].GetComponent<EnemyTurretController>().active == false)
        {
            turrets[2].GetComponent<EnemyTurretController>().active = true;
        }

        print("Enemy: " + gameObject.name + " Lowered health by " + amount + " health = " + health);
    }

}
