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
        SceneManager.LoadScene("mapScreen");
    }


}