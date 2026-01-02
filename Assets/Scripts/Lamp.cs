using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public LayerMask obstacle;

    private Rigidbody2D rb;

    public GameObject damager;

    public Animator animator;

    private bool falling;

    public GameObject lampShards;

    public AudioSource fallSound;

    private int canFall;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.isKinematic = true;

        canFall = Random.Range(0, 2);
    }

    private void Update()
    {
        if (falling)
        {
            animator.Play("LampFall");
        }
    }

    private bool LineOfSight(Vector2 start, Vector2 end, LayerMask obstacleMask)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, obstacleMask);
        return hit.collider == null;
    }

    public void FallTrigger()
    {
        Transform playerTransform = GameObject.Find("Player").transform;

        bool canSee = LineOfSight(transform.GetChild(0).position, playerTransform.position, obstacle);

        if (canSee && canFall == 1)
        {
            rb.isKinematic = false;
            damager.SetActive(true);
            fallSound.Play();
            falling = true;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }

    public void ExplodeBulb()
    {
        lampShards.SetActive(true);
        lampShards.transform.parent = null;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling)
        {
            ExplodeBulb();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (falling)
        {
            ExplodeBulb();
        }
    }
}