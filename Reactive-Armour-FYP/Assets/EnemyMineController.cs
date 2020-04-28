using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMineController : MonoBehaviour
{

    public GameObject lockedTarget;
    public GameObject player;
    public GameObject explosionSound;

    float rotationalDamp = 5f;

    public float fuseTimer = 20.0f;

    public bool latched = false;
    public bool ejected = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (latched != true)
        {
            checkTargetDistance();
            moveToTarget();

            if (lockedTarget != null && Vector3.Distance(transform.position, lockedTarget.transform.position) < 8.0f)
            {
                latched = true;
                fuseTimer = 10.0f;
            }


        }
        else if(ejected == false)
        {
            moveWithTarget();

            if(player.GetComponent<playerController>().guarding == true)
            {
                ejected = true;  
            }
        }

        fuseTimer -= Time.deltaTime;

        if (fuseTimer <= 0.0f)
        {
            explode();
        }
    }

    void checkTargetDistance()
    {
       
         float targetDistanceToMine = Vector3.Distance(transform.position, player.transform.position);
        if (targetDistanceToMine <= 100.0f)
        {
            lockedTarget = player;

        }

    }

    void moveToTarget()
    {
        if (lockedTarget != null)
        {
            transform.position += transform.forward * Time.deltaTime * 5.0f;
            turn();
        }

    }

    void moveWithTarget()
    {
        transform.rotation = lockedTarget.transform.rotation;
        transform.position = lockedTarget.transform.position + transform.forward * -2.0f;
    }

    void turn()
    {
        if (lockedTarget != null)
        {
            Vector3 pos = lockedTarget.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(pos);



            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        }
    }

    void explode()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, 30.0f);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject.tag == "player")
            {

                if (col.gameObject.GetComponent<playerController>() != null)
                {
                    col.gameObject.GetComponent<playerController>().lowerHealth(25);

                }


            }
            else
            if (col.gameObject.tag == "destructible")
            {
                col.gameObject.GetComponent<destructible>().lowerHealth(100);
            }
        }


        GameObject e = (GameObject)Instantiate(explosionSound, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
