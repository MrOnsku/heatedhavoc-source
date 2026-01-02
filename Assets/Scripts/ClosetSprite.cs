using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetSprite : MonoBehaviour
{
    public AudioSource fallSound;

    public void FallSound()
    {
        fallSound.Play();
    }
}