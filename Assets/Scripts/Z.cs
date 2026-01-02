using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z : MonoBehaviour
{
    private Vector2 center;
    private float randomOffset;

    private void Start()
    {
        center = transform.position;

        randomOffset = Random.Range(-100f, 100f);
    }

    private void Update()
    {
        transform.position = new Vector2(center.x + Mathf.Sin(randomOffset + Time.time) / 4, center.y);
        center.y += 0.5f * Time.deltaTime;
        center.x -= 0.1f * Time.deltaTime;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}