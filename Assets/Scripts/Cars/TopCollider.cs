using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OtherCar"))
        {
            if (UIManager.instance.position < UIManager.instance.totalPositions)
                UIManager.instance.position++;
            else
                UIManager.instance.position = UIManager.instance.totalPositions;

            UIManager.instance.UpdateSprintPostion();
        }
    }
}
