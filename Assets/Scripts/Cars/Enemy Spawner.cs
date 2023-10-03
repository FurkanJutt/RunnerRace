using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] carsPrefeb;
    public Transform[] xPositions;

    private int RandomChance;

    private void Start()
    {
        RandomChance = Random.Range(0, 4);
        CarsSpawn(xPositions[RandomChance]);
    }

    private void CarsSpawn(Transform xpos)
    {
        int rand = Random.Range(1, carsPrefeb.Length);
        Vector3 spawnPosition = new Vector3(xpos.position.x, xpos.position.y, xpos.position.z);
        Instantiate(carsPrefeb[rand], spawnPosition, Quaternion.identity, transform);
    }
}
