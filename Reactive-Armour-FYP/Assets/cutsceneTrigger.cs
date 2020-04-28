using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cutsceneTrigger : MonoBehaviour
{
    [TextArea(15, 20)]
    public List<string> windowText;
    [TextArea(15, 20)]
    public List<string> infoText;
    [TextArea(15, 20)]
    public List<string> dialogueText;

    public List<AudioClip> audioToPlay;
    public List<float> timings;


    public List<GameObject> triggerObjectsToBeDestroyed;

    public List<GameObject> triggerObjectsToBeActivated;

    //0 = trigger on destruction
    //1 = trigger on enter
    //2 = trigger on activation
    public int triggerType = 0;

    public GameObject player;
    public GameObject playerCamera;
    public GameObject cutsceneHUD;
    public GameObject HUD;

    public bool isPlaying = false;
    public bool currentTimingTriggered = false;

    public float currentTriggerTime = 0.0f;

    public int currentTrigger = -1;

    public GameObject cutsceneSpeaker;

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = (GameObject)Instantiate(cutsceneSpeaker, this.transform);
        cutsceneSpeaker = g;
        cutsceneSpeaker.GetComponent<cutsceneSpeaker>().audioToPlay = audioToPlay;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerType == 0 && isPlaying != true)
        {
            for (int i = 0; i < triggerObjectsToBeDestroyed.Count; i++)
            {
                if (triggerObjectsToBeDestroyed[i] == null)
                {
                    triggerObjectsToBeDestroyed.RemoveAt(i);
                }
            }


            if (triggerObjectsToBeDestroyed.Count == 0)
            {
                cutsceneSpeaker.transform.position = player.transform.position;
                isPlaying = true;
                Time.timeScale = 0;
                playerCamera.GetComponent<ThirdPersonCamera>().pausedGame = true;
                player.GetComponent<playerController>().pausedGame = true;
                HUD.SetActive(false);
                cutsceneHUD.SetActive(true);
            }
        }
        else if (triggerType == 2 && isPlaying != true)
        {
            for (int i = 0; i < triggerObjectsToBeActivated.Count; i++)
            {
                if (triggerObjectsToBeActivated[i].GetComponent<interactableScript>().isActivated == true)
                {
                    triggerObjectsToBeActivated.RemoveAt(i);
                }

                if (triggerObjectsToBeActivated.Count == 0)
                {
                    cutsceneSpeaker.transform.position = player.transform.position;
                    isPlaying = true;
                    Time.timeScale = 0;
                    playerCamera.GetComponent<ThirdPersonCamera>().pausedGame = true;
                    player.GetComponent<playerController>().pausedGame = true;
                    HUD.SetActive(false);
                    cutsceneHUD.SetActive(true);
                }
            }
        }




        if (isPlaying == true)
        {
            if (currentTimingTriggered == false)
            {
                if (currentTrigger < timings.Count -1)
                {
                    currentTrigger++;

                    GameObject.Find("IMGText").GetComponent<Text>().text = windowText[currentTrigger];
                    GameObject.Find("InfoText").GetComponent<Text>().text = infoText[currentTrigger];
                    GameObject.Find("TranscriptText").GetComponent<Text>().text = dialogueText[currentTrigger];
                    currentTriggerTime = 0;
                    currentTimingTriggered = true;
                    cutsceneSpeaker.GetComponent<cutsceneSpeaker>().playClip(currentTrigger);
                }
                else
                {
                    Time.timeScale = 1;
                    playerCamera.GetComponent<ThirdPersonCamera>().pausedGame = false;
                    player.GetComponent<playerController>().pausedGame = false;
                    cutsceneHUD.SetActive(false);
                    HUD.SetActive(true);

                    Destroy(cutsceneSpeaker);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                currentTriggerTime += Time.unscaledDeltaTime;

                if (currentTriggerTime >= timings[currentTrigger])
                {
                    currentTimingTriggered = false;
                }
            }

        }


        

    }


    public void OnTriggerEnter(Collider col)
    {
        if (triggerType == 1 && isPlaying == false)
        {
            cutsceneSpeaker.transform.position = player.transform.position;
            isPlaying = true;
            Time.timeScale = 0;
            playerCamera.GetComponent<ThirdPersonCamera>().pausedGame = true;
            player.GetComponent<playerController>().pausedGame = true;
            HUD.SetActive(false);
            cutsceneHUD.SetActive(true);
        }
    }



}
