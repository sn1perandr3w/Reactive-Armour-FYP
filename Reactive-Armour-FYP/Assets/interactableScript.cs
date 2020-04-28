using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class interactableScript : MonoBehaviour
{

    public bool isActivated;
    public float interactDistance = 20.0f;
    public bool singleUse = false;
    public bool singleUseActivated = false;

    //public bool activateObjects;
    public List<GameObject> objectsToToggle;


    //public bool destroysObjects;
    //public List<GameObject> objectsToDeactivate;

    public bool levelTransitionInteractable = false;
    public string levelToTransition = "";


    public bool awakenableInteractible = false;
    // Start is called before the first frame update

    void OnEnable()
    {
       GameObject p = GameObject.Find("PlayerCamera");

        p.GetComponent<ThirdPersonCamera>().interactables.Add(this.gameObject);

    }


    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        

    }



    public void interactableActivate()
    {
        if (singleUseActivated == false)
        {
            if (levelTransitionInteractable == true)
            {
                GameObject civController = GameObject.Find("civController");
                int civSaved = PlayerPrefs.GetInt("civSaved");

                civSaved += civController.GetComponent<civController>().actualCivAmount;
                PlayerPrefs.SetInt("civSaved", civSaved);
                PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);

                GameObject player = GameObject.FindGameObjectWithTag("player");

                PlayerPrefs.SetInt("healthSaved", player.GetComponent<playerController>().health);
                PlayerPrefs.SetInt("ammoSaved", player.GetComponent<playerController>().ammo);


                SceneManager.LoadScene(levelToTransition);
            }


            isActivated = true;

            for (int i = 0; i < objectsToToggle.Count; i++)
            {
                if (objectsToToggle[i].activeSelf == false)
                    objectsToToggle[i].SetActive(true);
                else { objectsToToggle[i].SetActive(false); }
            }


            if (singleUse)
            {
                
                singleUseActivated = true;
            }
        }
    }

    public void interactableDeactivate()
    {
        if (singleUseActivated == false)
        {
            isActivated = false;

            for (int i = 0; i < objectsToToggle.Count; i++)
            {
                if (objectsToToggle[i].activeSelf == false)
                    objectsToToggle[i].SetActive(true);
                else { objectsToToggle[i].SetActive(false); }
            }


            if (singleUse)
        {
            singleUseActivated = true;
        }
    }


}
}
