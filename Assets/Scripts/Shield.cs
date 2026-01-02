using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float centerx;
    private float centery;

    private void Update()
    {
        centerx = GameObject.Find("Player").transform.position.x;
        centery = GameObject.Find("Player").transform.position.y;

        transform.position = new Vector2(centerx + Mathf.Cos(Time.time), centery + Mathf.Sin(Time.time));
    }
}