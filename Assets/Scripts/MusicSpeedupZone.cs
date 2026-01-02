using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSpeedupZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.playerInMusicSpeedupZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.playerInMusicSpeedupZone = false;
        }
    }
}