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

    public Camera cam;

    Quaternion rotation = Quaternion.Euler(0, 0, 0);

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

    public List<GameObject> interactables;
    public List<GameObject> interactablesInRange;

    public int lockOnSelection = 0;

    float keyDownTime = 0.0f;

    Vector3 dir;
    Vector3 tgtDir;
    Vector3 tgtDir2;

    public float objxEuler;
    public float objyEuler;

    bool initialLockOn = false;

    Vector3 camRelative;

    bool initCloseCam = true;

    public bool pausedGame = false;

    public GameObject enemyUILabel;
    public GameObject enemyUIMarker;

    public GameObject hud;

    public List<GameObject> enemyLabels;
    public List<GameObject> enemyMarkers;

    public bool stopCamera = false;

    public bool zoom = false;

    public float zoomCooldown = 1.0f;

    public GameObject sniperCrosshair;

    private void Start()
    {
        aimTarget = GameObject.FindGameObjectWithTag("aimTarget");
        player = GameObject.FindGameObjectWithTag("player").transform;
        camTransform = transform;
        cam = this.GetComponent<Camera>();
        camTarget = aimTarget;
        playerTarget = aimTarget;

        hud = GameObject.Find("HUD");

        

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            enemies.Add(enemy);

            GameObject enemyMarker = (GameObject)Instantiate(enemyUIMarker);
            enemyMarker.transform.SetParent(hud.transform);

            enemyMarkers.Add(enemyMarker);


            GameObject enemyLabel = (GameObject)Instantiate(enemyUILabel);
            enemyLabel.transform.SetParent(hud.transform);
            if(enemy.GetComponent<enemyController>() != null)
            enemyLabel.GetComponent<Text>().text = enemy.GetComponent<enemyController>().enemyName;
            else if (enemy.GetComponent<EnemyMedicController>() != null)
            enemyLabel.GetComponent<Text>().text = enemy.GetComponent<EnemyMedicController>().enemyName;
            else if (enemy.GetComponent<EnemySniperController>() != null)
                enemyLabel.GetComponent<Text>().text = enemy.GetComponent<EnemySniperController>().enemyName;


            enemyLabels.Add(enemyLabel);

        }


        foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("OBJInteractable"))
        {
            interactables.Add(interactable);
        }

        midPointObj = new GameObject();
    }


    public void resetCooldown()
    {
        camResetCooldown = 2.0f;
    }


    private void Update()
    {
        if (pausedGame == false)
        {

            if (currentX > 360)
            {
                currentX -= 360;
            }
            else if (currentX <= 0)
            {
                currentX += 360;
            }

            //TOP LOOKING DOWN
            if (player.transform.eulerAngles.x >= 0 && player.transform.eulerAngles.x <= 70 && lockedOn)
            {
                currentY = (player.transform.eulerAngles.x);
            }
            else //BOTTOM LOOKING UP
            if (player.transform.eulerAngles.x <= 360 && player.transform.eulerAngles.x > 280 && lockedOn)
            {
                currentY = -(360 - player.transform.eulerAngles.x);
            }



            dir = new Vector3(0, 2, -distance);
            tgtDir = new Vector3(0, -10, -distance * 10);
            tgtDir2  = new Vector3(0, 0, -distance * 10);

            if (enemies.Count > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (enemy != null)
                    {


                        float enemyDistanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
                        if (enemyDistanceToPlayer <= 100.0f && !enemiesInLockOnRange.Contains(enemy) && (enemy.GetComponent<enemyController>() != null && enemy.GetComponent<enemyController>().grabbed == false || enemy.GetComponent<EnemyMedicController>()!= null && enemy.GetComponent<EnemyMedicController>().grabbed == false || enemy.GetComponent<EnemySniperController>() != null && enemy.GetComponent<EnemySniperController>().grabbed == false))
                        {
                            enemiesInLockOnRange.Add(enemy);
                            if (initialLockOn == true && zoom == false)
                            {
                                camTarget = enemiesInLockOnRange[0];
                                playerTarget = enemiesInLockOnRange[0];
                                if (enemiesInLockOnRange[0].GetComponent<EnemyMedicController>() != false)
                                {
                                    enemiesInLockOnRange[0].GetComponent<EnemyMedicController>().isTargeted = true;
                                }
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
                        Destroy(enemyMarkers[enemies.IndexOf(enemy)]);
                        enemyMarkers.RemoveAt(enemies.IndexOf(enemy));

                        Destroy(enemyLabels[enemies.IndexOf(enemy)]);
                        enemyLabels.RemoveAt(enemies.IndexOf(enemy));

                        enemies.Remove(enemy);
                        enemiesInLockOnRange.Remove(enemy);
                        
                    }
                }


                for (int i = 0; i < enemyMarkers.Count; i++)
                {
                    Vector3 enemyMarkerPos = this.GetComponent<Camera>().WorldToScreenPoint(enemies[i].transform.position + (transform.up * 1.0f));
                    Vector3 enemyLabelPos = enemyMarkerPos + (transform.up * 32.0f);
                    enemyMarkers[i].transform.position = enemyMarkerPos;
                    enemyLabels[i].transform.position = enemyLabelPos;

                    

                    if (enemiesInLockOnRange.Contains(enemies[i]) && enemies[i] != playerTarget)
                    {
                        //print("UI CHECK 1");
                        enemyMarkers[i].GetComponent<RawImage>().color = new Color32(255, 153, 0, 255);
                        enemyLabels[i].GetComponent<Text>().color = new Color32(255, 153, 0, 255);
                    }

                    
                    else if (enemies[i] == playerTarget)
                    {
                        //print("UI CHECK 2");
                        enemyMarkers[i].GetComponent<RawImage>().color = new Color32(255, 0, 12, 255);
                        enemyLabels[i].GetComponent<Text>().color = new Color32(255, 0, 12, 255);
                    }

                    else
                    {
                        //print("UI CHECK 3");
                        enemyMarkers[i].GetComponent<RawImage>().color = new Color32(0, 255, 138, 255);
                        enemyLabels[i].GetComponent<Text>().color = new Color32(0, 255, 138, 255);
                    }
                    
                }
            }





            if (!interactables.Equals(0))
            {
                foreach (GameObject interactable in interactables)
                {

                    float interactableDistanceToPlayer = Vector3.Distance(player.transform.position, interactable.transform.position);
                    float interactableDistance = interactable.GetComponent<interactableScript>().interactDistance;
                    if (interactableDistanceToPlayer <= interactableDistance)
                    {
                        

                        if (!interactablesInRange.Contains(interactable))
                            interactablesInRange.Add(interactable);
                    }
                    else if (interactableDistanceToPlayer > interactableDistance)
                    {
                        


                        if (interactablesInRange.Contains(interactable))
                            interactablesInRange.Remove(interactable);
                    }
                }


                if (interactablesInRange.Count > 0)
                {
                    GameObject.Find("UIInteractPrompt").GetComponent<Text>().text = "Press X to Interact";
                }
                else
                {
                    GameObject.Find("UIInteractPrompt").GetComponent<Text>().text = "";
                }
            }
            else
            {
                GameObject.Find("UIInteractPrompt").GetComponent<Text>().text = "";
            }
        


        GameObject.Find("UIEnemyCount").GetComponent<Text>().text = "Enemies in Area: " + enemies.Count;

        if (Input.GetKey(KeyCode.X) && interactablesInRange.Count > 0)
        {
            foreach (GameObject interactable in interactablesInRange)
            {
                if(interactable.GetComponent<interactableScript>().isActivated == false)
                interactable.GetComponent<interactableScript>().interactableActivate();

                else
                interactable.GetComponent<interactableScript>().interactableDeactivate();
            }
        }


            if (Input.GetKey(KeyCode.F) && lockedOn == false && zoomCooldown <= 0.0f)
            {
                if (zoom == false)
                {
                    cam.fieldOfView = 10;
                    sniperCrosshair.SetActive(true);
                    sensitivityX = 12.0f;
                    sensitivityY = 12.0f;
                    zoom = true;
                }
                else
                {
                    cam.fieldOfView = 60;
                    sniperCrosshair.SetActive(false);
                    sensitivityX = 48.0f;
                    sensitivityY = 48.0f;
                    zoom = false;
                }
                zoomCooldown = 1.0f;
            }


            if (Input.GetKey(KeyCode.Q) && lockedOn == true)
        {

            keyDownTime += Time.deltaTime;

            if (keyDownTime >= 1.0f || playerTarget == null && enemiesInLockOnRange.Count == 0)
            {
                    resetCamera();
            }
        }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                keyDownTime = 0;
            }

        if (Input.GetKeyDown(KeyCode.E) && zoom == false)
        {
                if (enemiesInLockOnRange.Count > 0)
                {
                    if(camTarget.GetComponent<EnemyMedicController>() != null)
                    camTarget.GetComponent<EnemyMedicController>().isTargeted = false;

                    camTarget = enemiesInLockOnRange[lockOnSelection];
                    playerTarget = enemiesInLockOnRange[lockOnSelection];

                    if (camTarget.GetComponent<EnemyMedicController>() != null)
                        camTarget.GetComponent<EnemyMedicController>().isTargeted = true;

                    if ((lockOnSelection + 1) != enemiesInLockOnRange.Count)
                    {
                        lockOnSelection++;
                    }
                    else
                    {
                        lockOnSelection = 0;
                    }

                    keyDownTime = 0.0f;
                }
                
        }


        if (enemiesInLockOnRange.Count.Equals(0))
        {
            initialLockOn = true;
        }


        if (playerTarget != null)
        {
            distanceBetweenPlayerandTarget = Vector3.Distance(player.transform.position, playerTarget.transform.position);
            playerTargetPos = playerTarget.transform.position;
        }
            //print ("DISTANCE FROM PLAYER TO TARGET = " + distanceBetweenPlayerandTarget);

            
            //print("CURRENT X = " + currentX);
            //print("Current Euler X = " + player.transform.eulerAngles.y);
            //print("CURRENT Y = " + currentY);
            //print("Current Euler Y = " + (player.transform.eulerAngles.x));
            
            /*
            if (currentY >= 0)
            {
                print("Current Euler Y = " + ( player.transform.eulerAngles.x));
            }
            else
            {
            print("Current Euler Y = " + (-(360 - player.transform.eulerAngles.x)));
            }
            */

            if (lockedOn == false && stopCamera == false)
        {
            currentX += Input.GetAxis("Mouse X");
            currentY -= Input.GetAxis("Mouse Y");
        }
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        if (camTarget != aimTarget)
        {
            lockedOn = true;
        }

        objxEuler = player.transform.eulerAngles.y;
        objyEuler = player.transform.eulerAngles.x;

        if (camTarget == null || playerTarget == null)
        {
            /*
            if (this.transform.eulerAngles.x > 281)
            {
                currentY = this.transform.eulerAngles.x - 360;
            }
            else
            {
                currentY = this.transform.eulerAngles.x;
            }
            currentX = this.transform.eulerAngles.y;
            */
            if (enemiesInLockOnRange.Count.Equals(0))
            {
                camTarget = aimTarget;
                playerTarget = aimTarget;
                lockedOn = false;
                camLockedCombat = false;
                lockOnSelection = 0;

                    resetCamera();
            }
            else
            {
                camTarget = enemiesInLockOnRange[0];
                playerTarget = enemiesInLockOnRange[0];
                lockOnSelection = 0;
            }
        }

        if (camResetCooldown > 0.0f && distanceBetweenPlayerandTarget >= 10.0f)
        {
            camResetCooldown -= Time.deltaTime;
        }
        else
        {
            camLockedCombat = false;
        }
        }


        if (zoomCooldown > 0.0f)
        {
            zoomCooldown -= Time.deltaTime;
        }


    }


    private void LateUpdate()
    {
        if (pausedGame == false)
        {

            if (Time.timeScale != 0)
            {



                if (lockedOn == false)
                {
                    //print ("NOT LOCKED ON BUT ROTATING");
                    rotation = Quaternion.Euler(currentY, currentX, 0);
                    playerTarget = aimTarget;
                }
                else if (lockedOn == true && camLockedCombat == false)
                {
                    //print ("LOCKED ON BUT ROTATING");
                    rotation = Quaternion.Euler(player.transform.eulerAngles);
                }


                if (distanceBetweenPlayerandTarget < 10.0f || camLockedCombat == true)
                {


                    if (playerTarget != null)
                    {
                        midpointPos = (player.transform.position + playerTarget.transform.position) * 0.5f;




                        if (initCloseCam == true)
                        {
                            camRelative = midpointPos - camTransform.position;
                            //print ("MID = " + midpointPos + "CAM = " + camTransform.position + "RELATIVE = " + camRelative);
                            initCloseCam = false;
                        }

                        var smoothRotation = Quaternion.LookRotation(midpointPos - camTransform.position);

                        camTransform.rotation = Quaternion.Slerp(camTransform.rotation, smoothRotation, Time.deltaTime * sensitivityX / 4);
                        camTransform.position = Vector3.Slerp(camTransform.position, (midpointPos - camRelative), 0.7f);

                        //camTransform.rotation = smoothRotation;
                        //camTransform.position = midpointPos - camRelative;

                        camResetCooldown = 2.0f;

                        

                        aimTarget.transform.position = player.position - rotation * tgtDir2;
                        //aimTarget.transform.position = player.position - rotation * tgtDir;

                    }

                    else
                    {
                        resetCamera();
                    }


                }
                else if (camLockedCombat == false && camTarget != null )
                {
                    initCloseCam = true;
                    var smoothRotation = Quaternion.LookRotation(camTarget.transform.position - camTransform.position);

                    camTransform.rotation = Quaternion.Slerp(camTransform.rotation, smoothRotation, Time.deltaTime * sensitivityX);
                    camTransform.position = Vector3.Slerp(camTransform.position, player.position + rotation * dir, 0.7f);

                    //camTransform.rotation = smoothRotation;
                    //camTransform.position = player.position + rotation * dir;

                    aimTarget.transform.position = player.position - rotation * tgtDir2; 
                }

                


                player.LookAt(playerTargetPos);
            }
            if (playerTarget != null && playerTarget.tag == "enemy")
            {

                if(playerTarget.GetComponent<enemyController>() != null)
                GameObject.Find("UIEnemyInfo").GetComponent<Text>().text = "Enemy Health: " + playerTarget.GetComponent<enemyController>().health +"%";
                else
                    if (playerTarget.GetComponent<EnemyMedicController>() != null)
                    GameObject.Find("UIEnemyInfo").GetComponent<Text>().text = "Enemy Health: " + playerTarget.GetComponent<EnemyMedicController>().health + "%";
                else
                    if (playerTarget.GetComponent<EnemySniperController>() != null)
                    GameObject.Find("UIEnemyInfo").GetComponent<Text>().text = "Enemy Health: " + playerTarget.GetComponent<EnemySniperController>().health + "%";
            }
            else
            {
                GameObject.Find("UIEnemyInfo").GetComponent<Text>().text = "Enemy Health: N/A";
            }
        }
    }

    public void grabCameraReset()
    {
        if (enemiesInLockOnRange.Count > 0)
        {
            
            
            if ((lockOnSelection + 1) < enemiesInLockOnRange.Count)
            {
                lockOnSelection++;
            }
            else
            {
                lockOnSelection = 0;
            }

            camTarget = enemiesInLockOnRange[lockOnSelection];
            playerTarget = enemiesInLockOnRange[lockOnSelection];

            keyDownTime = 0.0f;
            

            
        }

        else
        {
            resetCamera();
        }
    }

    public void throwCameraReset()
    {
        /*
        if (enemiesInLockOnRange.Count > 0)
        {
            camTarget = enemiesInLockOnRange[lockOnSelection];
            playerTarget = enemiesInLockOnRange[lockOnSelection];
            if ((lockOnSelection + 1) != enemiesInLockOnRange.Count)
            {
                lockOnSelection++;
            }
            else
            {
                lockOnSelection = 0;
            }
            keyDownTime = 0.0f;
        }

        else
        {
            resetCamera();
        }
        */

        if (enemiesInLockOnRange.Count == 1)
        {
            lockedOn = true;
            //camTarget = enemiesInLockOnRange[0];
            playerTarget = enemiesInLockOnRange[0];
            lockOnSelection = 0;
        }

        for (int i = 0; i < enemyMarkers.Count; i++)
        {
            Vector3 enemyMarkerPos = this.GetComponent<Camera>().WorldToScreenPoint(enemies[i].transform.position + (transform.up * 1.0f));
            Vector3 enemyLabelPos = enemyMarkerPos + (transform.up * 32.0f);
            enemyMarkers[i].transform.position = enemyMarkerPos;
            enemyLabels[i].transform.position = enemyLabelPos;



        }

    }

    public void resetCamera()
    {
        //aimTarget.transform.position = player.position - rotation * tgtDir;

        

        aimTarget.transform.position = player.position - rotation * tgtDir2;

        print("CURRENT X = " + currentX);
        print("Current Euler X = " + player.transform.eulerAngles.y);
        print("CURRENT Y = " + currentY);

        if (currentY >= 0)
        {
            print("Current Euler Y = " + (player.transform.eulerAngles.x));
        }
        else
        {
            print("Current Euler Y = " + (-(360 + player.transform.eulerAngles.x)));
        }


        currentX = player.transform.eulerAngles.y;


        if (camTarget.GetComponent<EnemyMedicController>() != null)
        {
            
            camTarget.GetComponent<EnemyMedicController>().isTargeted = false;
        }


        camTarget = aimTarget;
        playerTarget = aimTarget;
        lockedOn = false;
        camLockedCombat = false;

        //camTransform.rotation = Quaternion.Euler(camTarget.transform.position - aimTarget.transform.position);
        //camTransform.position = player.position + dir;

        keyDownTime = 0;
    }

}
