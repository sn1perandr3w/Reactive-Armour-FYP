using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneSpeaker : MonoBehaviour
{
    // Start is called before the first frame update


    public List<AudioClip> audioToPlay;
    AudioSource audioSource;


    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void playClip(int i)
    {
        audioSource.clip = audioToPlay[i];
        audioSource.Play();

    }
}
