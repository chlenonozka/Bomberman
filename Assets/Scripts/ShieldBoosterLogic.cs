using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBoosterLogic : MonoBehaviour
{
    public GameObject Shield;
    public int rotationSpeed;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().getShielded() == false)
            {
                Destroy(gameObject);
            }
            else { return; }
        }
    }
}
