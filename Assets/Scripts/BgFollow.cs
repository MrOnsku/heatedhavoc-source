using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgFollow : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = new Vector2(transform.position.x, Camera.main.transform.position.y);
    }
}