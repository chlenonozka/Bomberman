using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLogic : MonoBehaviour
{
    public AudioSource aux;

    void Update()
    {
        if (aux.volume > 0)
        {
            aux.volume -= 0.5f * Time.deltaTime / 0.5f;
        }
    }
}
