using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dependentDestructible : MonoBehaviour
{

    public List<GameObject> dependedObjects;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < dependedObjects.Count; i++)
        {
            if (dependedObjects[i] == null)
            {
                dependedObjects.RemoveAt(i);
            }
        }

        if (dependedObjects.Count == 0)
        {
            Destroy(this.gameObject);
        }


    }
}
