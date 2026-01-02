using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //generate more
            LevelGen.instance.CreateFloor();
        }
    }
}