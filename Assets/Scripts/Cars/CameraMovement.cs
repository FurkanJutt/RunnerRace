using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public float cameraSpeed = 0.04f;
	//public Rigidbody thisrgbd;
	//public float speed;
	private void Update ()
	{
		transform.position += Time.deltaTime * Vector3.up * cameraSpeed;
	}

}