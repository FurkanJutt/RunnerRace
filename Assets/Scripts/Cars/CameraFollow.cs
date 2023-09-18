using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
	private Vector3 offset;
	private float y;
	public float speedFollow = 5f;

	// Use this for initialization
	void Start () 
    {

		player = GameObject.FindGameObjectWithTag("Player").transform;
		offset = transform.position - player.position;

	}
	
	// Update is called once per frame
	void LateUpdate()
{
    // Get the player's current position
    Vector3 playerPosition = player.position;

    // Update the camera's position only on the Y-axis to follow the player's Y position
    Vector3 cameraPosition = transform.position;
    cameraPosition.y = playerPosition.y + offset.y;
    transform.position = cameraPosition;
}

}
