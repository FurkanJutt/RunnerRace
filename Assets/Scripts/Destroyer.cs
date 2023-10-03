using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin") || collision.CompareTag("Timer") || collision.CompareTag("Opponent"))
        {
            Destroy(collision.gameObject);
        }
    }
}
