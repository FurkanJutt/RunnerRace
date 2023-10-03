using ControlFreak2.Demos.Guns;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("OtherCar"))
        {
            if (UIManager.instance.position > 1)
                UIManager.instance.position--;
            else
                UIManager.instance.position = 1;

            UIManager.instance.UpdateSprintPostion();
        }
    }
}
