using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public static CameraMovement instance;

	public float cameraSpeed = 0.04f;
    //public Rigidbody thisrgbd;
    //public float speed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update ()
	{
		transform.position += Time.deltaTime * Vector3.up * cameraSpeed;
	}

}