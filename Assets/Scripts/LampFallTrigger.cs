using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFallTrigger : MonoBehaviour
{
    public Lamp lamp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lamp.FallTrigger();
        }
    }
}