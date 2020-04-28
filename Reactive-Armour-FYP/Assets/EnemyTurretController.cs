using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretController : MonoBehaviour
{

    public GameObject boss;

    public GameObject player;
    public GameObject turretBody;
    public GameObject barrelPivot;

    public GameObject projectile;

    public bool active;

    public float attackCooldown = 0.0f;

    public float depression = 0.0f;
    public float elevation = 0.0f;

    public float clampedElevation;

    public AudioSource shotSource;
    public AudioClip shotSound;


    // Start is called before the first frame update
    void Start()
    {
        shotSource.clip = shotSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {

            if (boss == null)
            {
                active = false;
            }


            Vector3 targetTurretPos = new Vector3(player.transform.position.x,
                                        this.transform.position.y,
                                        player.transform.position.z);
            this.transform.LookAt(targetTurretPos);

            Vector3 targetGunPos = new Vector3(player.transform.position.x,
                                        player.transform.position.y,
                                        player.transform.position.z) - barrelPivot.transform.position;
            //barrelPivot.transform.LookAt(targetGunPos);

            var barrelRot = Quaternion.LookRotation(targetGunPos);

            //barrelRot.eulerAngles = new Vector3(Mathf.Clamp(barrelRot.eulerAngles.x, depression, elevation), barrelRot.eulerAngles.y, barrelRot.eulerAngles.z);

            clampedElevation = barrelRot.eulerAngles.x;

            if (barrelRot.eulerAngles.x < (360 - elevation) && barrelRot.eulerAngles.x > 180)
            {
                clampedElevation = 360 - elevation;
            }
            else if (barrelRot.eulerAngles.x > depression && barrelRot.eulerAngles.x < 180)
            {
                clampedElevation = depression;
            }
            else if (attackCooldown <= 0.0f)
            {
                shoot();
            }


            barrelRot.eulerAngles = new Vector3(clampedElevation, this.gameObject.transform.eulerAngles.y, barrelRot.eulerAngles.z);
            //barrelRot.eulerAngles = new Vector3(barrelRot.eulerAngles.x, barrelRot.eulerAngles.y, barrelRot.eulerAngles.z);



            barrelPivot.transform.rotation = Quaternion.Slerp(barrelPivot.transform.rotation, barrelRot, Time.deltaTime * 5);
            //this.gameObject.transform.LookAt(player.transform);
            //barrelPivot.transform.LookAt(player.transform);

            if (attackCooldown > 0.0f)
            {
                attackCooldown -= Time.deltaTime;
            }



        }
    }


    void shoot()
    {
        Vector3 xyz = new Vector3(barrelPivot.transform.eulerAngles.x, barrelPivot.transform.eulerAngles.y, barrelPivot.transform.eulerAngles.z);
        Quaternion newRotation = Quaternion.Euler(xyz);


        GameObject g = (GameObject)Instantiate(projectile, barrelPivot.transform.position + barrelPivot.transform.forward * 62.0f, newRotation);
            g.GetComponent<Rigidbody>().AddForce(barrelPivot.transform.forward * 8000);
            Destroy(g, 5);
            attackCooldown = 6.0f;

        shotSource.Play();
        
    }
}
