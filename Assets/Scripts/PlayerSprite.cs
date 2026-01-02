using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    public AudioSource stepAudioSource;
    public AudioClip stepSound;

    public void Step()
    {
        stepAudioSource.pitch = Random.Range(0.5f, 1.5f);
        stepAudioSource.PlayOneShot(stepSound);
    }
}