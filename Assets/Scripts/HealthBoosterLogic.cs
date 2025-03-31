using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoosterLogic : MonoBehaviour
{
    public GameObject Health;
    public int rotationSpeed;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().getHealth() > 0 && other.GetComponent<Player>().getHealth() != 3)
            {
                Destroy(gameObject);
            }
            else { return; }
        }
    }
}
