using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class projectileScript : MonoBehaviour {


    public bool allyProjectile = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll)
	{

       

        //Removes health from Player.
        if (allyProjectile != true)
        {
            if (coll.gameObject.tag == "player")
            {

                coll.gameObject.GetComponent<playerController>().lowerHealth(20);
            }
            else
            if (coll.gameObject.tag == "destructible")
            {
                print("HITTING CIVILIANS");
                coll.gameObject.GetComponent<destructible>().lowerHealth(20);
            }
            else if (coll.gameObject.tag == "ally")
            {
                print("HITTING ALLY");
                coll.gameObject.GetComponent<AllySecurityMechController>().lowerHealth(20);
            }
        }
        else
        {
            if (coll.gameObject.tag == "enemy")
            {
                if (coll.gameObject.GetComponent<enemyController>() != null)
                {
                    coll.gameObject.GetComponent<enemyController>().lowerHealth(100);

                }

                else if (coll.gameObject.GetComponent<EnemyMedicController>() != null)
                {
                    coll.gameObject.GetComponent<EnemyMedicController>().lowerHealth(100);

                }
                else if (coll.gameObject.GetComponent<EnemySniperController>() != null)
                {
                    coll.gameObject.GetComponent<EnemySniperController>().lowerHealth(100);

                }


            }
        }
			
		Object.Destroy (this.gameObject);
	}
		

}
