using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public Animator animator;

    public GameObject damager;

    public bool active;

    public float cycle = 3;

    private void Start()
    {
        if(cycle == 0)
        {
            cycle = 3;
        }

        StartCoroutine("StartFlame");
    }

    private void Update()
    {
        if (active)
        {
            animator.Play("FlamethrowerActive");
            damager.SetActive(true);
        }
        else
        {
            animator.Play("FlamethrowerIdle");
            damager.SetActive(false);
        }
    }

    private IEnumerator StartFlame()
    {
        yield return new WaitForSeconds(cycle);

        active = !active;

        yield return new WaitForSeconds(cycle);

        active = !active;

        StartCoroutine("StartFlame");
    }
}