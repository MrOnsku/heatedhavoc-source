using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetDamage : MonoBehaviour
{
    public GameObject damager;
    public GameObject dust;

    public void EnableDamager()
    {
        damager.SetActive(true);
        dust.SetActive(true);
    }

    public void DisableDamager()
    {
        damager.SetActive(false);
    }
}