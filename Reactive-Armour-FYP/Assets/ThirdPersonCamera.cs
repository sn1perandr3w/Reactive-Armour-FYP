using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonCamera : MonoBehaviour
{

	private const float Y_ANGLE_MIN = -70.0f;
	private const float Y_ANGLE_MAX = 70.0f;

	public GameObject aimTarget;
	public Transform camTransform;

	public Transform player;

	public GameObject camTarget;

	public GameObject playerTarget;

	private Camera cam;

	Quaternion rotation = Quaternion.Euler (0, 0, 0);

	private float distance = 7.0f;
	public float currentX = 0.0f;
	public float currentY = 0.0f;
	private float sensitivityX = 48.0f;
	private float sensitivityY = 48.0f;

	private float camResetCooldown = 2.0f;

	bool lockedOn = false;

	bool camLockedCombat = false;

	float distanceBetweenPlayerandTarget = 0.0f;

	private Vector3 midpointPos;
	private GameObject midPointObj;
	private Vector3 playerTargetPos;

	public List<GameObject> enemies;
	public List<GameObject> enemiesInLockOnRange;

	int lockOnSelection = 0;

	float keyDownTime = 0.0f;

	Vector3 dir;
	Vector3 tgtDir;

	public float objxEuler;
	public float objyEuler;

	bool initialLockOn = false;

	Vector3 camRelative;

	bool initCloseCam = true;



	private void Start ()
	{
		aimTarget = GameObject.FindGameObjectWithTag("aimTarget");
		player = GameObject.FindGameObjectWithTag("player").transform;
		camTransform = transform;
		cam = Camera.main;
		camTarget = aimTarget;
		playerTarget = aimTarget;

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")) {
			enemies.Add (enemy);
		}

		midPointObj = new GameObject();
	}


	public void resetCooldown ()
	{
		camResetCooldown = 2.0f;
	}


	private void Update ()
	{

		dir = new Vector3 (0, 2, -distance);
		tgtDir = new Vector3 (0, -10, -distance * 10);

        if (!enemies.Equals(0))
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    float enemyDistanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
                    if (enemyDistanceToPlayer <= 100.0f && !enemiesInLockOnRange.Contains(enemy))
                    {
                        enemiesInLockOnRange.Add(enemy);
                        if (initialLockOn == true)
                        {
                            camTarget = enemiesInLockOnRange[0];
                            playerTarget = enemiesInLockOnRange[0];
                            initialLockOn = false;
                        }
                    }
                    else if (enemyDistanceToPlayer > 100.0f && enemiesInLockOnRange.Contains(enemy))
                    {
                        enemiesInLockOnRange.Remove(enemy);
                    }
                }
                else if (enemy == null)
                {
                    enemies.Remove(enemy);
                    enemiesInLockOnRange.Remove(enemy);
                }
            }
        }

		GameObject.Find ("UIEnemyCount").GetComponent<Text> ().text = "Enemies in Area: " + enemies.Count;


		if (Input.GetKey (KeyCode.Q) && lockedOn == true) {

			keyDownTime += Time.deltaTime;

			if (keyDownTime >= 1.0f) {
				if (this.transform.eulerAngles.x > 281) {
					currentY = this.transform.eulerAngles.x - 360;
				} else {
					currentY = this.transform.eulerAngles.x;
				}
				currentX = this.transform.eulerAngles.y;


				camTarget = aimTarget;
				playerTarget = aimTarget;
				lockedOn = false;
				camLockedCombat = false;
				keyDownTime = 0;
			}
		}



		if (Input.GetKeyDown (KeyCode.E)) {

			camTarget = enemiesInLockOnRange [lockOnSelection];
			playerTarget = enemiesInLockOnRange [lockOnSelection];
			if ((lockOnSelection + 1) != enemiesInLockOnRange.Count) {
				lockOnSelection++;
			} else {
				lockOnSelection = 0;
			}
			keyDownTime = 0.0f;
		}


		if (enemiesInLockOnRange.Count.Equals (0)) {
			initialLockOn = true;
		}


		if (playerTarget != null) {
			distanceBetweenPlayerandTarget = Vector3.Distance (player.transform.position, playerTarget.transform.position);
			playerTargetPos = playerTarget.transform.position;
		}
		//print ("DISTANCE FROM PLAYER TO TARGET = " + distanceBetweenPlayerandTarget);


		if (lockedOn == false) {
			currentX += Input.GetAxis ("Mouse X");
			currentY -= Input.GetAxis ("Mouse Y");
		}
		currentY = Mathf.Clamp (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		
		if (camTarget != aimTarget) {
			lockedOn = true;
		}

		objxEuler = player.transform.eulerAngles.y;
		objyEuler = player.transform.eulerAngles.x;

		if (camTarget == null || playerTarget == null) {

			if (this.transform.eulerAngles.x > 281) {
				currentY = this.transform.eulerAngles.x - 360;
			} else {
				currentY = this.transform.eulerAngles.x;
			}
			currentX = this.transform.eulerAngles.y;

			if (enemiesInLockOnRange.Count.Equals (0)) {
				camTarget = aimTarget;
				playerTarget = aimTarget;
				lockedOn = false;
				camLockedCombat = false;
				lockOnSelection = 0;
			} else 
			{
				camTarget = enemiesInLockOnRange [0];
				playerTarget = enemiesInLockOnRange [0];
				lockOnSelection = 0;
			}
		}

		if (camResetCooldown > 0.0f && distanceBetweenPlayerandTarget >= 10.0f) {
			camResetCooldown -= Time.deltaTime;
		} else {
			camLockedCombat = false;
		}


	}

	private void LateUpdate ()
	{
		if (Time.timeScale != 0) {
			


			if (lockedOn == false) {
				//print ("NOT LOCKED ON BUT ROTATING");
				rotation = Quaternion.Euler (currentY, currentX, 0);
				playerTarget = aimTarget;
			} else if (lockedOn == true && camLockedCombat == false) {
				//print ("LOCKED ON BUT ROTATING");
				rotation = Quaternion.Euler (player.transform.eulerAngles);
			}
				

			if (distanceBetweenPlayerandTarget < 10.0f || camLockedCombat == true) {
				
				
				if (playerTarget != null) {
					midpointPos = (player.transform.position + playerTarget.transform.position) * 0.5f;

				}

				if (initCloseCam == true) 
				{
					camRelative = midpointPos - camTransform.position;
					//print ("MID = " + midpointPos + "CAM = " + camTransform.position + "RELATIVE = " + camRelative);
					initCloseCam = false;
				}

				var smoothRotation = Quaternion.LookRotation(midpointPos - camTransform.position);
				camTransform.rotation = Quaternion.Slerp(camTransform.rotation, smoothRotation, Time.deltaTime * sensitivityX/4);
				camTransform.position = Vector3.Slerp(camTransform.position, (midpointPos - camRelative), 0.7f);
				camResetCooldown = 2.0f;


			} else if (camLockedCombat == false && camTarget != null) {
				initCloseCam = true;
				var smoothRotation = Quaternion.LookRotation(camTarget.transform.position - camTransform.position);
				camTransform.rotation = Quaternion.Slerp(camTransform.rotation, smoothRotation, Time.deltaTime * sensitivityX);
				camTransform.position = Vector3.Slerp(camTransform.position, player.position + rotation * dir, 0.7f);
			}

			aimTarget.transform.position = player.position - rotation * tgtDir;


			player.LookAt (playerTargetPos);
		}
		if (playerTarget != null && playerTarget.tag == "enemy") {
			GameObject.Find ("UIEnemyInfo").GetComponent<Text> ().text = "Enemy: " + playerTarget.GetComponent<enemyController> ().enemyName;
		} else 
		{
			GameObject.Find ("UIEnemyInfo").GetComponent<Text> ().text = "Enemy: NONE";
		}
	}

}
