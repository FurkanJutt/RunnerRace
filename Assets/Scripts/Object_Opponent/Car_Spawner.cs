using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    public GameObject[] carsPrefeb;
    public float[] xPositions;
    public Transform parentTransform;

    private void Start()
    {
        StartCoroutine(CarsSpawner());
    }

    private void CarsSpawn()
    {
        int rand = Random.Range(0, carsPrefeb.Length);
        float randomX = GetRandomXPosition();
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);
        Instantiate(carsPrefeb[rand], spawnPosition, Quaternion.identity, parentTransform);
    }

    private float GetRandomXPosition()
    {
        int index = Random.Range(0, xPositions.Length);
        return xPositions[index];
    }

    private IEnumerator CarsSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CarsSpawn();
        }
    }
}
