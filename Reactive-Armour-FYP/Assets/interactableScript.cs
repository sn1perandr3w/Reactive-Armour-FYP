using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableScript : MonoBehaviour
{

    public bool isActivated;


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
        isActivated = true;
    }

    public void interactableDeactivate()
    {
        isActivated = false;
    }
}
