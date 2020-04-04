using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AllySecurityMechController : MonoBehaviour
{
    public Transform enemyTarget;
    public GameObject target;
    public Transform targetStored;
    public int healthLimit;
    public int health;

    public GameObject explosionSound;

    public string enemyName;

    float rotationalDamp = 60.0f;

    float movementSpeed = 10.0f;
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

    public GameObject[] patrolWaypoints;
    public int waypointNum = 0;

    public GameObject escortTarget;


    public float attackCooldown = 0.0f;


    int selection;

    public GameObject projectile;



    public string triggerForReturn;

 

    public float timeUntilEvade = 0.0f;

    public List<GameObject> enemies;
    public List<GameObject> enemiesInAttackRange;


    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        if (patrolWaypoints.Length > 0)
        {
            anim.SetTrigger("patrol");
            target = patrolWaypoints[0];
        }
        else if (escortTarget != null)
        {
            anim.SetTrigger("escort");
            target = escortTarget;
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            enemies.Add(enemy);

        }



    }

    // Update is called once per frame
    void Update()
    {


        info = anim.GetCurrentAnimatorStateInfo(0);

        Debug.DrawRay(transform.position, transform.forward * 3.0f, Color.blue);

        //print("Target = " + target);

        if (pursueBuffer >= 0.0f)
        {
            pursueBuffer -= Time.deltaTime;
        }

        

        if (info.IsName("Idle"))
        {
            if (patrolWaypoints.Length > 0)
            {
                anim.SetTrigger("patrol");
                target = patrolWaypoints[0];
            }
            else if (escortTarget != null)
            {
                anim.SetTrigger("escort");
                target = escortTarget;
            }
        }
        

        if (attackCooldown > 0.0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (info.IsName("Patrol"))
        {
            Move();
            checkWaypointDistance();
        }
        else if (info.IsName("Escort"))
        {
            Move();
        }


        else if (info.IsName("Combat"))
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                //print("Distance = " + distance);
                if (distance < 50.0f)
                {
                    transform.LookAt(target.transform);

                    ceaseMove();
                    if (attackCooldown <= 0.0f)
                    {
                        triggerForReturn = "combat";
                        anim.SetTrigger("ranged");
                    }
                }
                else if (distance >= 50.0f)
                {
                    Move();
                }

            } else if (target == null)
            {

                enemies.Remove(target);
                enemiesInAttackRange.Remove(target);

                if (enemiesInAttackRange.Count > 0)
                {
                    target = enemiesInAttackRange[0];
                }
                else
                {
                    anim.SetTrigger("idle");
                }
            }

        }

        else if (info.IsName("RangedAttack"))
        {
            //print ("RANGED ATTACKING");
            rangedAttack();
        }

        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {


                    if (enemiesInAttackRange.Contains(enemies[i]))
                        enemiesInAttackRange.Remove(enemies[i]);

                    enemies.Remove(enemies[i]);
                }
                else if(enemies[i] != null)
                {
                   
                


                if (Vector3.Distance(transform.position, enemies[i].transform.position) <= 50 && !enemiesInAttackRange.Contains(enemies[i]))
                {
                    enemiesInAttackRange.Add(enemies[i]);
                }
                else if (Vector3.Distance(transform.position, enemies[i].transform.position) > 50 && enemiesInAttackRange.Contains(enemies[i]))
                {
                    enemiesInAttackRange.Remove(enemies[i]);
                }

                if (enemiesInAttackRange.Count > 0 && !info.IsName("Combat") && !info.IsName("RangedAttack"))
                {
                    target = enemies[i];
                    anim.SetTrigger("combat");
                }
                }
            }
        }

    }


    //Creates a ranged attack using a raycast.

    public void rangedAttack()
    {
        if (attackCooldown <= 0.0f)
        {
            print("RANGED ATTACK");
            GameObject g = (GameObject)Instantiate(projectile, (transform.position + (transform.forward * 3.0f) + (transform.up * 1.0f)), Quaternion.identity);
            g.GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position).normalized * 4000);
            g.name = "ALLYPROJECTILE";
            Destroy(g, 5);
            attackCooldown = 6.0f;
        }
        //newSelection();
        anim.ResetTrigger("ranged");
        anim.SetTrigger(triggerForReturn);

    }



    //Advances NPC towards target.

    void Move()
    {
        //print("Moving");
        if (GetComponent<NavMeshAgent>().isStopped == true)
        {
            GetComponent<NavMeshAgent>().isStopped = false;
        }
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
    }

    void ceaseMove()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
    }


    void checkWaypointDistance()
    {


        Vector3 pos = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, target.transform.position);


        if (target.transform.tag == "wayPoint")
        {

            if (distance < 3.0f)
            {
                if (waypointNum == patrolWaypoints.Length - 1)
                {
                    waypointNum = 0;
                }
                else
                {
                    waypointNum++;
                }
            }
            target = patrolWaypoints[waypointNum];


        }
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

            GameObject e = (GameObject)Instantiate(explosionSound, transform.position, Quaternion.identity);
            

            Destroy(this.gameObject);
        }


        print("Ally: " + gameObject.name + " Lowered health by " + amount + " health = " + health);
    }
}
