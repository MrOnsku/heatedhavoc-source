using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public static Phone instance;

    public Animator animator;

    private void Start()
    {
        instance = this;
    }
}