using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapDemo : MonoBehaviour {

    //This script goes on the SpikeTrap prefab;

    public Animator spikeTrapAnim; //Animator for the SpikeTrap;

    public AudioSource sound;
    public AudioSource sound2;

    // Use this for initialization
    void Awake()
    {
        //get the Animator component from the trap;
        spikeTrapAnim = GetComponent<Animator>();
        //start opening and closing the trap for demo purposes;
        StartCoroutine(OpenCloseTrap());
    }


    IEnumerator OpenCloseTrap()
    {
        //play open animation;
        spikeTrapAnim.SetTrigger("open");
        sound.Play();
        //wait 2 seconds;
        yield return new WaitForSeconds(4);
        //play close animation;
        spikeTrapAnim.SetTrigger("close");
        sound2.Play();
        //wait 2 seconds;
        yield return new WaitForSeconds(5);
        //Do it again;
        StartCoroutine(OpenCloseTrap());

    }
}