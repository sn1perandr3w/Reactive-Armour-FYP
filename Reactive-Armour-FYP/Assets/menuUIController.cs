using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class menuUIController : MonoBehaviour
{

    public GameObject civSavedText;
    public GameObject healthSavedText;
    public GameObject ammoSavedText;
    public List<string> levelClearFlags;

    public List<GameObject> levelButtons;
    public List<GameObject> nextLevelButtons;
    public List<string> nextLevelToMark;
    public bool levelMarked = false;

    // Start is called before the first frame update
    void Start()
    {
        civSavedText.GetComponent<Text>().text = "Civilians Saved: " + PlayerPrefs.GetInt("civSaved");
        healthSavedText.GetComponent<Text>().text = "Health: " + PlayerPrefs.GetInt("healthSaved");
        ammoSavedText.GetComponent<Text>().text = "Ammo: " + PlayerPrefs.GetInt("ammoSaved");


        for (int i = 0; i < levelClearFlags.Count; i++)
        {
            if (PlayerPrefs.GetInt(levelClearFlags[i]) == 1)
            {
                levelButtons[i].GetComponent<Image>().color = new Color32(0, 255, 138, 62);
            }
        }
        
        for (int i = 0; i < nextLevelToMark.Count; i++)
        {
            if (PlayerPrefs.GetInt(nextLevelToMark[i]) == 0 && levelMarked == false)
            {
                nextLevelButtons[i].GetComponent<Image>().color = new Color32(255, 250, 0, 62);
                levelMarked = true;
            }
        }
        

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
