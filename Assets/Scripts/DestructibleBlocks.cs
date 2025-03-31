using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlocks : MonoBehaviour
{
    public GameObject explosionPrefab;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            Explode();
        }

        if (other.CompareTag("Explosion_super"))
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject prefab = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject, .1f);
        //Destroy(gameObject);
    }
}
