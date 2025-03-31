using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombPickUp_Spawner : MonoBehaviour
{
    public GameObject pickUp_Bomb_Prefab;
    public int spawnCount = 0;
    public int maxSpawnCount;
    public float rangeX;
    public float rangeY;
    public float minTime, maxTime;
    public Vector3[] spawnPositionsV;

    public float timer;
    public float timerPoint;
    public bool pauseTimer;

    public GameObject inGamePickUp;

    private void Update()
    {
        if(pauseTimer == false)
        {
            timer += Time.deltaTime;
        }

        if (inGamePickUp == null && timer > timerPoint)
        {
            timer = 0;
            pauseTimer = true;
            StartCoroutine(spawn());
        }
        else { return; }
    }

    private IEnumerator spawn()
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        if (spawnCount < maxSpawnCount && !inGamePickUp)
        {
            int spawnIndex = Random.Range(0, spawnPositionsV.Length);

            Vector3 spawnPosition = spawnPositionsV[spawnIndex]; //new Vector3(Mathf.RoundToInt(UnityEngine.Random.Range(-rangeX, rangeX)), 0, Mathf.RoundToInt(UnityEngine.Random.Range(-rangeY, rangeY)));
            inGamePickUp = Instantiate(pickUp_Bomb_Prefab, spawnPosition, Quaternion.identity);
            spawnCount++;

            pauseTimer = false;
        }
    }
}
