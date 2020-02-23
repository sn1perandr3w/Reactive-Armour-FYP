using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clusterBombScript : MonoBehaviour
{
    public Vector3 offsetPosition;

    public GameObject player;

    public GameObject explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void explode()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(this.transform.position, 5.0f);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject.tag == "enemy")
            {
                col.gameObject.GetComponent<enemyController>().lowerHealth(10);
                col.gameObject.GetComponent<enemyController>().initKnockBack(player.transform, 0.2f, 40.0f);
            }
            else
            if (col.gameObject.tag == "destructible")
            {
                col.gameObject.GetComponent<destructible>().lowerHealth(25);
            }
        }


        GameObject e = (GameObject)Instantiate(explosionSound, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
