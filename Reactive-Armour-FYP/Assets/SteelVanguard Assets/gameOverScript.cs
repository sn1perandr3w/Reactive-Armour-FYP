using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gameOverScript : MonoBehaviour {

	//Used to control the menu in the game over screen.

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //Makes scene change to new, specified scene.
    public void gameOver()
    {
        SceneManager.LoadScene("splashScreen");
    }
}
