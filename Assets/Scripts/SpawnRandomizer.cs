using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomizer : MonoBehaviour
{
    void Start()
    {
        var random = Random.Range(0, 2);

        if(random == 1)
        {
            Destroy(gameObject);
        }
    }
}