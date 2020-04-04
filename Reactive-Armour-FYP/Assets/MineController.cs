using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{

    public GameObject lockedTarget;
    public List<GameObject> targets;
    public List<GameObject> targetsInLockOnRange;
    public GameObject explosionSound;

    float rotationalDamp = 5f;

    float fuseTimer = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("enemy"))
        {
            targets.Add(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkForTargets();
        moveToTarget();


        if (fuseTimer <= 0.0f || lockedTarget != null && Vector3.Distance(transform.position, lockedTarget.transform.position) < 8.0f)
        {
            explode();
        }
    }

    void checkForTargets()
    {
        if (targets.Count > 0)
        {
            foreach (GameObject target in targets)
            {
                if (target != null)
                {
                    float targetDistanceToMine = Vector3.Distance(transform.position, target.transform.position);
                    if (targetDistanceToMine <= 100.0f && !targetsInLockOnRange.Contains(target))
                    {
                        targetsInLockOnRange.Add(target);
                        
                    }
                    else if (targetDistanceToMine > 100.0f && targetsInLockOnRange.Contains(target))
                    {
                        targetsInLockOnRange.Remove(target);
                    }
                }
                else if (target == null)
                {
                    targets.Remove(target);
                    targetsInLockOnRange.Remove(target);
                }
            }

            if (targetsInLockOnRange.Count > 0)
            {
                lockedTarget = targetsInLockOnRange[0];
            }
        }
    }

    void moveToTarget()
    {
        if (lockedTarget != null)
        {
            transform.position += transform.forward * Time.deltaTime * 5.0f;
            turn();

            fuseTimer -= Time.deltaTime;
        }
        
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
            if (col.gameObject.tag == "enemy")
            {
               
                if (col.gameObject.GetComponent<enemyController>() != null)
                {
                    col.gameObject.GetComponent<enemyController>().lowerHealth(100);
                    
                }

                else if (col.gameObject.GetComponent<EnemyMedicController>() != null)
                {
                    col.gameObject.GetComponent<EnemyMedicController>().lowerHealth(100);
                    
                }
                else if (col.gameObject.GetComponent<EnemySniperController>() != null)
                {
                    col.gameObject.GetComponent<EnemySniperController>().lowerHealth(100);
                    
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
