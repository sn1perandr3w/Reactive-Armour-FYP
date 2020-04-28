using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndGameUIController : MonoBehaviour
{

    public GameObject civSavedText;

    // Start is called before the first frame update
    void Start()
    {
        civSavedText.GetComponent<Text>().text = "Civilians Saved: " + PlayerPrefs.GetInt("civSaved");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
