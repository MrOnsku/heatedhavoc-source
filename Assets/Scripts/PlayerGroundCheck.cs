using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            player.grounded = true;
        }

        if(collision.CompareTag("Slippery"))
        {
            player.slipping = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            player.damageKnockback = false;
            player.damageFx.SetActive(false);
        }

        if (collision.CompareTag("Slippery"))
        {
            player.slipping = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            player.grounded = false;
        }

        if (collision.CompareTag("Slippery"))
        {
            player.slipping = false;
        }
    }
}