﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHitBox : MonoBehaviour
{

    public List<GameObject> enemiesToHit;
    public List<GameObject> enemiesHit;
    float timeUntilStop;
    bool activeHitbox;
    int comboHit;

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
                //print("HITLOOP");
                foreach (GameObject enemy in enemiesToHit)
                {
                    //print("HITLOOP2");
                    if(enemy != null && !enemiesHit.Contains(enemy)) {

                        //print("COMBO HIT = " + comboHit);

                        if (comboHit >= 4)
                        {
                            //print("KNOCKBACK FROM HITBOX");
                            //enemy.GetComponent<enemyController>().initKnockBack(transform.parent, 0.2f, 40.0f);

                            if (enemy.gameObject.GetComponent<enemyController>() != null)
                            {
                                enemy.gameObject.GetComponent<enemyController>().initKnockBack(transform.parent, 0.2f, 40.0f);

                            }

                            else if (enemy.gameObject.GetComponent<EnemyMedicController>() != null)
                            {
                                enemy.gameObject.GetComponent<EnemyMedicController>().initKnockBack(transform.parent, 0.2f, 40.0f);

                            }
                            else if (enemy.gameObject.GetComponent<EnemySniperController>() != null)
                            {
                                enemy.gameObject.GetComponent<EnemySniperController>().initKnockBack(transform.parent, 0.2f, 40.0f);

                            }
                            




                        }
                        else
                        {
                            //print("STUN FROM HITBOX");
                            //enemy.GetComponent<enemyController>().initStun(0.1f);


                            if (enemy.gameObject.GetComponent<enemyController>() != null)
                            {
                                enemy.gameObject.GetComponent<enemyController>().initStun(0.1f);

                            }

                            else if (enemy.gameObject.GetComponent<EnemyMedicController>() != null)
                            {
                                enemy.gameObject.GetComponent<EnemyMedicController>().initStun(0.1f);

                            }
                            else if (enemy.gameObject.GetComponent<EnemySniperController>() != null)
                            {
                                enemy.gameObject.GetComponent<EnemySniperController>().initStun(0.1f);

                            }


                        }

                        if (enemy.gameObject.GetComponent<enemyController>() != null)
                        {
                            enemy.gameObject.GetComponent<enemyController>().lowerHealth(damage);

                        }

                        else if (enemy.gameObject.GetComponent<EnemyMedicController>() != null)
                        {
                            enemy.gameObject.GetComponent<EnemyMedicController>().lowerHealth(damage);

                        }
                        else if (enemy.gameObject.GetComponent<EnemySniperController>() != null)
                        {
                            enemy.gameObject.GetComponent<EnemySniperController>().lowerHealth(damage);

                        }

                        else if (enemy.gameObject.GetComponent<EnemyCombatEngineerController>() != null)
                        {
                            enemy.gameObject.GetComponent<EnemyCombatEngineerController>().lowerHealth(damage);

                        }
                        else if (enemy.gameObject.GetComponent<destructible>() != null)
                        {
                            enemy.gameObject.GetComponent<destructible>().lowerHealth(damage);

                        }


                        enemiesHit.Add(enemy);
                        }
                }
            }
            

        }


        if (timeUntilStop <= 0 && activeHitbox == true)
        {
            //print("CLEARING");
            enemiesHit.Clear();
            enemiesToHit.Clear();
            hitboxInactive();
        }

        
    }


    private void OnTriggerStay(Collider collidedObject)
    {
       //print("TR COLLIDING WITH " + collidedObject);
        if ((collidedObject.gameObject.tag == "enemy" || collidedObject.gameObject.tag == "destructible") && activeHitbox)
        {
            if (!enemiesToHit.Contains(collidedObject.gameObject))
            {
                enemiesToHit.Add(collidedObject.gameObject);
            }
        }
    }
    /*
    private void OnCollisionEnter(Collision collidedObject)
    {
        print("C COLLIDING WITH " + collidedObject);
        if ((collidedObject.gameObject.tag == "destructible") && activeHitbox)
        {
            if (!enemiesToHit.Contains(collidedObject.gameObject))
            {
                enemiesToHit.Add(collidedObject.gameObject);
            }
        }
    }
    */

    /*
    private void OnControllerColliderHit(ControllerColliderHit collidedObject)
    {
       
            print("CCH COLLIDING WITH " + collidedObject);
            if ((collidedObject.gameObject.tag == "destructible") && activeHitbox)
            {
                if (!enemiesToHit.Contains(collidedObject.gameObject))
                {
                    enemiesToHit.Add(collidedObject.gameObject);
                }
            }
      
    }
    */
    public void hitboxInactive()
    {
        
        //print("DEACTIVATING");
        activeHitbox = false;
    }

    public void hitboxActive(int knockBack, float stunTime,float timeUntilStop, int damage, int comboHit)
    {
        this.knockBack = knockBack;
        this.stunTime = stunTime;
        this.damage = damage;
        this.timeUntilStop = timeUntilStop;
        this.comboHit = comboHit;
        activeHitbox = true;
        //print("ACTIVATING");
    }
}
