using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectiveTriggerSet : MonoBehaviour
{
    public GameObject interactible;
    public string triggerToSet;
    public int triggerValue;

    //0 = active to deactive
    //1 = deactive to active
    public int activateOrDeactivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activateOrDeactivate == 0 && interactible.GetComponent<interactableScript>().isActivated == false)
        {
            PlayerPrefs.SetInt(triggerToSet, triggerValue);
        }
        else if (activateOrDeactivate == 1 && interactible.GetComponent<interactableScript>().isActivated == true)
        {
            PlayerPrefs.SetInt(triggerToSet, triggerValue);
        }

    }
}
