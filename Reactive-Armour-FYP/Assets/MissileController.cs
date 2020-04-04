using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{

    float rotationalDamp = 2.5f;

    public GameObject target;

    float turnDelay = 1.0f;

    public int missileNo;

    float launchTimer = 0.1f;

    float safetyTimer = 5.0f;

    bool safetyTimerEngaged = false;

    public GameObject explosionSound;

    //Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.Find("CivilianCrowd");

        //rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (launchTimer < 0.0f)
        {
            move();
            turn();
            checkDetonationDistance();
        }
        else
        {
            lowerLaunchTimer();
        }
    }

    void lowerLaunchTimer()
    {
        launchTimer -= Time.deltaTime;
    }

    public void setLaunchTimer()
    {
        launchTimer = launchTimer * missileNo;
    }

    void checkDetonationDistance()
    {
        if (!safetyTimerEngaged)
        {
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 5.0f)
                {
                    explode();

                }
            }
            else
            {
                Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, 80.0f);
                foreach (Collider col in objectsInRange)
                {
                    if (col.gameObject.tag == "enemy")
                    {
                        print("MISSILE SELECTED NEW TARGET");
                        target = col.gameObject;
                    }

                }

                if (target == null)
                {
                    safetyTimerEngaged = true;
                }
            }
        }
        else
        {
            lowerSafetyTimer();

            if (safetyTimer <= 0)
            {
                print("MISSILE SAFETY TIMER");
                explode();
            }
        }
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("MISSILE COLLIDED");

        explode();
    }

    void explode()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, 8.0f);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject.tag == "enemy")
            {
                if (col.gameObject.GetComponent<enemyController>() != null)
                {
                    col.gameObject.GetComponent<enemyController>().lowerHealth(50);
                   
                }

                else if (col.gameObject.GetComponent<EnemyMedicController>() != null)
                {
                    col.gameObject.GetComponent<EnemyMedicController>().lowerHealth(50);
                   
                }
                else if (col.gameObject.GetComponent<EnemySniperController>() != null)
                {
                    col.gameObject.GetComponent<EnemySniperController>().lowerHealth(50);
                   
                }
            }
            else
            if(col.gameObject.tag == "destructible")
            {
                col.gameObject.GetComponent<destructible>().lowerHealth(50);
            }
        }
        GameObject e = (GameObject)Instantiate(explosionSound, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void move()
    {
        transform.position += transform.forward * Time.deltaTime * 10.0f;
    }


    void lowerSafetyTimer()
    {
        safetyTimer -= Time.deltaTime;
    }
    void turn()
    {
        if (target != null && turnDelay <= 0.0f)
        {
            Vector3 pos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(pos);



            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        }
        else
        {
            turnDelay -= Time.deltaTime;
        }
    }
}
