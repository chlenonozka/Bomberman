using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject explosionPrefab;
    public LayerMask levelMask;
    public LayerMask levelMask2;
    public float explosionTime;
    private bool exploded = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("Explode", explosionTime);
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));
        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        transform.Find("Collider").gameObject.SetActive(false);
        Destroy(gameObject, .3f);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < 3; i++)
        {
            RaycastHit hit;
            RaycastHit hit2;
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit,
            i, levelMask);
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit2,
            i, levelMask2);

            if (!hit.collider && !hit2.collider)
            {
                GameObject prefab = Instantiate(explosionPrefab, transform.position + (i * direction),
                explosionPrefab.transform.rotation);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode();
        }

        if(!exploded && other.CompareTag("Player"))
        {
            rb.AddForce(transform.up, ForceMode.Impulse);
        }
    }
}
