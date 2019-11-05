using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBox : MonoBehaviour
{

    public List<GameObject> enemiesToHit;
    public List<GameObject> enemiesHit;
    float timeUntilStop;
    bool activeHitbox;

    int damage;
    int knockBack;
    float stunTime;


    void start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilStop -= Time.deltaTime;

        //print(timeUntilStop);
        if (timeUntilStop > 0.0f)
        {
            if (enemiesToHit.Count > 0)
            {
                print("HITLOOP");
                foreach (GameObject enemy in enemiesToHit)
                {
                    print("HITLOOP2");
                    if(enemy != null && !enemiesHit.Contains(enemy)) {
                        
                        enemy.GetComponent<enemyController>().KnockBack(transform.parent, 0.2f, 15.0f);
                        enemiesHit.Add(enemy);
                        }
                }
            }
            

        }


        if (timeUntilStop <= 0 && activeHitbox == true)
        {
            print("CLEARING");
            enemiesHit.Clear();
            enemiesToHit.Clear();
            hitboxInactive();
        }

        
    }


    private void OnTriggerStay(Collider collidedObject)
    {
        print("COLLIDING WITH " + collidedObject);
        if (collidedObject.gameObject.tag == "enemy" && activeHitbox)
        {
            if (!enemiesToHit.Contains(collidedObject.gameObject))
            {
                enemiesToHit.Add(collidedObject.gameObject);
            }
        }
    }

    public void hitboxInactive()
    {
        
        print("DEACTIVATING");
        activeHitbox = false;
    }

    public void hitboxActive(int knockBack, float stunTime,float timeUntilStop, int damage)
    {
        this.knockBack = knockBack;
        this.stunTime = stunTime;
        this.damage = damage;
        this.timeUntilStop = timeUntilStop;
        activeHitbox = true;
        print("ACTIVATING");
    }
}
