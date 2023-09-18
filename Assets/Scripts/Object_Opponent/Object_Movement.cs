using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ControlFreak2.TouchControl;

public class Object_Movement : MonoBehaviour
{
    private Transform objectTransform;

    private float speed = 2f;
    public float minSpeed = 2f;
    public float maxSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        objectTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        Clamp();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Grass") || collider.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }

    private void Clamp()
    {
        //Unity Inbuilt Feature
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.20f, 2.20f);
       // pos.y = Mathf.Clamp(pos.y, -3.8f, 3.8f);
        transform.position = pos;
    }
}
