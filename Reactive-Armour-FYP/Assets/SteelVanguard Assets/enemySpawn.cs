using UnityEngine;
using System.Collections;

public class enemySpawn : MonoBehaviour
{
	//Declaration of variables/objects.

    public GameObject Enemy;
	float distance;
	float time = 0.0f;
	int spawn;
	public static int totalEnemyAmount;

 
    void Start()
    {
		//Ensures that a spawnpoint may or may not spawn an enemy once the game starts.
		spawn = Random.Range(1,3);

		if(spawn == 2){
            Instantiate(Enemy, transform.position + Vector3.right * 3.0f, Quaternion.identity);
			totalEnemyAmount = totalEnemyAmount + 1;
       }
    }
    // Update is called once per frame
    void Update()
    {
		//Enemies will respawn unless the player is too close to the spawnpoint.
		distance = Vector3.Distance (transform.position, GameObject.Find ("chestTarget").transform.position);

		if (distance > 60.0f && totalEnemyAmount < 7) 
		{
			time += Time.deltaTime;

			if (time > 30.0f) 
			{
				totalEnemyAmount = totalEnemyAmount + 1;
				Instantiate(Enemy, transform.position + Vector3.right * 3.0f, Quaternion.identity);
				time = 0.0f;
			}
		}
    }
}
