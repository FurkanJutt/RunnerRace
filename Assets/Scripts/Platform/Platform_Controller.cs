using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Controller : MonoBehaviour
{
    public GameObject[] collects;

    public Vector2 numberOfCollects;

    public List<GameObject> newCollects;

    private void Start()
    {
       int newNumberOfCollects = (int)Random.Range(numberOfCollects.x, numberOfCollects.y);

		for (int i = 0; i < newNumberOfCollects; i++)
		{
			newCollects.Add(Instantiate(collects[Random.Range(0, collects.Length)], transform));
			newCollects[i].SetActive(false);
		}

        PositionateCollects();
    }

	private void PositionateCollects()
    {
		for (int i = 0; i < newCollects.Count; i++)
		{
			float posYMin = (51f / newCollects.Count) + (51f / newCollects.Count) * i;
			float posYMax = (51f / newCollects.Count) + (51f / newCollects.Count) * i + 1;
			newCollects[i].transform.localPosition = new Vector3(0, Random.Range(posYMin, posYMax), 0);
			newCollects[i].SetActive(true);

			if (newCollects[i].GetComponent<ChangeLane>() != null)
				newCollects[i].GetComponent<ChangeLane>().PositionLane();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other !=null && other.CompareTag("Destroyer"))
		{
			transform.position = new Vector3(0, transform.position.y + 51 * 3, 0);
            PositionateCollects();
		}
	}
}
