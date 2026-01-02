using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogblock : MonoBehaviour
{
    public GameObject destructionParticles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Dog")
        {
            destructionParticles.SetActive(true);
            destructionParticles.transform.parent = null;
            Destroy(gameObject);
        }
    }
}