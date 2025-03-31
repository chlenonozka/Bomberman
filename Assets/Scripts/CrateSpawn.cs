using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawn : MonoBehaviour
{
    public GameObject crate;
    public Vector3[] spawnPositions;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= spawnPositions.Length; i++)
        {
            GameObject prefab = Instantiate(crate, spawnPositions[i], Quaternion.identity);
        }
    }

    public void respawnCrates()
    {
        for (int i = 0; i <= spawnPositions.Length; i++)
        {
            GameObject prefab = Instantiate(crate, spawnPositions[i], Quaternion.identity);
        }
    }
}
