using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class persistenceController : MonoBehaviour
{

    public List<GameObject> persistentObjects;


    public List<int> objectState;

    public string persistenceState;


    // Start is called before the first frame update
    void Start()
    {

       

        persistenceState = PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "Persistence");

        

        for (int i = 0; i < persistentObjects.Count; i++)
        {
            //print("PERSISTENT STATE = " + persistenceState[i]);
            //print("INT OF PERSISTENT STATE = " + int.Parse(persistenceState[i].ToString()));
            objectState[i] = int.Parse(persistenceState[i].ToString());
        }

        //print("PERSISTENCE STATE = " + persistenceState);

        for (int i = 0; i < persistentObjects.Count; i++)
        {
            //print("OBJECT STATE = " + objectState[i]);
            if (objectState[i] == 0)
            {
                //print("DESTROYING " + i);
                Destroy(persistentObjects[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        updatePersistence();
    }


    public void updatePersistence()
    {
        for (int i = 0; i < persistentObjects.Count; i++)
        {

            if (persistentObjects[i] == null)
            {
                objectState[i] = 0;
            }
            else
            {
                objectState[i] = 1;
            }
        }

        persistenceState = "";

        for (int i = 0; i < persistentObjects.Count; i++)
        {
            persistenceState += objectState[i];
        }

        savePersistence();
    }

    public void savePersistence()
    {
        //print("SAVING: " + persistenceState);
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "Persistence", persistenceState);
    }
}
