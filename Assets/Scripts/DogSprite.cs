using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSprite : MonoBehaviour
{
    public AudioSource stepSound;

    public void StepSound()
    {
        stepSound.pitch = Random.Range(0.5f, 1.5f);
        stepSound.Play();
    }
}