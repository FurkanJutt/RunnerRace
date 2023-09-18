using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Spawner : MonoBehaviour
{
    public GameObject[] objectPrefeb;
    public float[] xPositions; 

    private void Start()
    {
        StartCoroutine(ObjectsSpawner());
    }

    private void ObjectsSpawn()
    {
        int rand = Random.Range(0, objectPrefeb.Length);
        float randomX = GetRandomXPosition();
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);
        Instantiate(objectPrefeb[rand], spawnPosition, Quaternion.identity);
    }

    private float GetRandomXPosition()
    {
        int index = Random.Range(0, xPositions.Length);
        return xPositions[index];
    }

    private IEnumerator ObjectsSpawner()
    {
        while (true)
        {
            // Adjust the WaitForSeconds duration as needed
            yield return new WaitForSeconds(2f);
            ObjectsSpawn();
        }
    }
}
