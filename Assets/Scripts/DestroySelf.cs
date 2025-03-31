using UnityEngine;
using System.Collections;

/// <summary>
/// Small script for easily destroying an object after a while
/// </summary>
public class DestroySelf : MonoBehaviour
{
    public float Delay = 4f;
    private AudioSource globalStateManager;
    //Delay in seconds before destroying the gameobject

    void Start ()
    {
        /* globalStateManager.Play();
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Destroy (gameObject, Delay); */
        StartCoroutine(explodeDestroy());
    }

    private IEnumerator explodeDestroy()
    {
        globalStateManager = GameObject.FindWithTag("GameController").GetComponent<AudioSource>();
        globalStateManager.Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(Delay - 1);
        Destroy(gameObject);
    }
}
