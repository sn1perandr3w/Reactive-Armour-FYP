using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class pauseMenuScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Exits to map screen
    public void exitArea()
    {
        Time.timeScale = 1;

        GameObject civController = GameObject.Find("civController");
        int civSaved = PlayerPrefs.GetInt("civSaved");
            
          civSaved  += civController.GetComponent<civController>().actualCivAmount;
        PlayerPrefs.SetInt("civSaved", civSaved);
        PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);

        GameObject player = GameObject.FindGameObjectWithTag("player");

        PlayerPrefs.SetInt("healthSaved", player.GetComponent<playerController>().health);
        PlayerPrefs.SetInt("ammoSaved", player.GetComponent<playerController>().ammo);

        SceneManager.LoadScene("mapScreen");
    }


    public void vrReset()
    {
        GameObject persistenceController = GameObject.Find("PersistenceController");
        Destroy(persistenceController);

        

        PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "Persistence", "11");

        Time.timeScale = 1;
        SceneManager.LoadScene("testRoom");
    }


}