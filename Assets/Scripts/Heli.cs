using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heli : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip propellerSound;

    public void Sound()
    {
        audioSource.PlayOneShot(propellerSound);
        audioSource.volume -= 0.015f;
    }
}