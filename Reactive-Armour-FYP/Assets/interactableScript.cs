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

    // Start is called before the first frame update
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
