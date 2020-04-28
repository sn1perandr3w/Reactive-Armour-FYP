using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateAllies : MonoBehaviour
{

    public List<GameObject> alliesToActivate;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("allyMechsFound") == 1)
        {
            for (int i = 0; i < alliesToActivate.Count; i++)
            {
                alliesToActivate[i].SetActive(true);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
