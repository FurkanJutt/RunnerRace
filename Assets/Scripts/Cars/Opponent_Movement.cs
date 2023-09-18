using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent_Movement : MonoBehaviour
{
    public float speed = 2f;

    private void Update()
    {
        VerticalMovement();
    }

    private void VerticalMovement()
    {
        // Move the opponent vertically
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Timer"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Coin"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }
        // else if (other.CompareTag("Water"))
        // {
        //     Time.timeScale = 0f;
        //     UIManager.instance.gameOverPanel.SetActive(true);
        //     TimerController.instance.DisplayTotalTimePlayed();
        // }
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
