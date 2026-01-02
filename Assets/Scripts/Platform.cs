using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public SpriteRenderer sprite;

    private void Start()
    {
        sprite.color = new Color(1, 1, 1, 0);
    }
}