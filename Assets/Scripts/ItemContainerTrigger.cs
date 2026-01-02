using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerTrigger : MonoBehaviour
{
    public ItemContainer itemContainer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            itemContainer.Buy();
        }
    }
}