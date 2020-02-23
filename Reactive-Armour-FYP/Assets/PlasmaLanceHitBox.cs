using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaLanceHitBox : MonoBehaviour
{
    public List<GameObject> enemiesToHit;
    public List<float> enemyInvincibility;
    public List<GameObject> enemiesHit;
    public bool activeHitbox;
    int damageLevel;

    int damage;
    int knockBack;
    float stunTime;


    void start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (activeHitbox)
        {
            if (enemiesToHit.Count > 0)
            {
               


                print("HITLOOP");
                for (int i = 0; i < enemiesToHit.Count; i++)
                {
                    print("HITLOOP2");
                    if (enemiesToHit[i] != null && enemyInvincibility[i] <= 0.0f)
                    {

                        if (damageLevel == 2)
                        {
                            //print("KNOCKBACK FROM HITBOX");
                            enemiesToHit[i].GetComponent<enemyController>().initKnockBack(transform.parent, 1.0f, 40.0f);
                        }
                        else
                        {
                            //print("STUN FROM HITBOX");
                            enemiesToHit[i].GetComponent<enemyController>().initStun(0.2f);
                        }
                        enemiesToHit[i].GetComponent<enemyController>().lowerHealth(damage);
                        enemyInvincibility[i] = 0.5f;
                    }
                    else if (enemiesToHit[i] == null)
                    {
                        enemiesToHit.RemoveAt(i);
                        enemyInvincibility.RemoveAt(i);
                    }
                }
            }


        }


        for (int i = 0; i < enemyInvincibility.Count; i++)
        {
            enemyInvincibility[i] -= Time.deltaTime;
        }


        /*
    if (timeUntilStop <= 0 && activeHitbox == true)
    {
        print("CLEARING");
        enemiesHit.Clear();
        enemiesToHit.Clear();
        hitboxInactive();
    }
    */

    }


    private void OnTriggerStay(Collider collidedObject)
    {
        //print("COLLIDING WITH " + collidedObject);
        if (collidedObject.gameObject.tag == "enemy" && activeHitbox)
        {
            if (!enemiesToHit.Contains(collidedObject.gameObject))
            {
                enemyInvincibility.Add(0.0f);
                enemiesToHit.Add(collidedObject.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider collidedObject)
    {
        //print("COLLIDING WITH " + collidedObject);
        if (collidedObject.gameObject.tag == "enemy")
        {
            if (enemiesToHit.Contains(collidedObject.gameObject))
            {
                enemyInvincibility.RemoveAt(enemiesToHit.IndexOf(collidedObject.gameObject));
                enemiesToHit.Remove(collidedObject.gameObject);
            }
        }
    }

    public void hitboxInactive()
    {

        print("DEACTIVATING");
        activeHitbox = false;
    }

    public void hitboxActive(int knockBack, float stunTime, int damage, int damageLevel)
    {
        this.knockBack = knockBack;
        this.stunTime = stunTime;
        this.damage = damage;
        this.damageLevel = damageLevel;

        activeHitbox = true;
        print("ACTIVATING");
    }
}

