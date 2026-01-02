using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public string itemName;

    public int price;

    public Canvas canvas;
    public TMP_Text text;

    public GameObject sprite;
    public BoxCollider2D colliderr;

    private bool bought = false;

    public AudioSource audioSource;
    public AudioClip noMoney;

    private void Start()
    {
        price += Random.Range(-1, 5);
    }

    private void Update()
    {
        if(canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }

        if (!bought)
        {
            text.text = itemName + "\n$" + price.ToString();
        }
    }

    public void Buy()
    {
        Debug.Log("Buy");

        if (!bought)
        {
            if (GameManager.instance.coins >= price)
            {
                bought = true;
                sprite.SetActive(false);
                colliderr.enabled = false;
                GameManager.instance.coins -= price;
                text.text = "Thank you for your purchase!";
            }
            else
            {
                audioSource.PlayOneShot(noMoney);
            }
        }
    }
}