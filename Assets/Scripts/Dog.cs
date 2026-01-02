using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    private Rigidbody2D rb;

    public Animator animator;
    public SpriteRenderer sprite;

    public float speed;
    public float smoothing;

    private float xSmoothed;

    public GameObject zPrefab;
    public Transform zSpawnPos;

    private bool zSpawnInit;

    public LayerMask obstacle;

    private bool inRange;

    private RaycastHit2D sight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 6f)
        {
            inRange = true;
            Debug.Log("In range");
        }
        else
        {
            inRange = false;
        }

        var playerPosition = GameObject.Find("Player").transform.position;

        sight = Physics2D.Raycast((Vector2)transform.position, ((Vector2)GameObject.Find("Player").transform.position - (Vector2)transform.position).normalized, Mathf.Infinity, obstacle);

        bool canSeePlayer = inRange && LineOfSight(transform.position, playerPosition, obstacle);

        if (canSeePlayer)
        {
            Debug.Log("In sight");
        }

        if (canSeePlayer)
        {
            Chase();
            animator.Play("DogChase");

            StopCoroutine("SpawnZ");

            zSpawnInit = false;
        }
        else
        {
            xSmoothed = Mathf.Lerp(xSmoothed, 0, (smoothing * 2) * Time.deltaTime);
            animator.Play("DogSleep");

            if (zSpawnInit == false)
            {
                StartCoroutine("SpawnZ");
                zSpawnInit = true;
            }
        }

        rb.linearVelocity = new Vector2(xSmoothed, rb.linearVelocity.y);
    }

    private void Chase()
    {
        Vector2 target = GameObject.FindGameObjectWithTag("Player").transform.position;

        animator.speed = Mathf.Abs(rb.linearVelocity.x) / 6f;

        if (target.x >= transform.position.x)
        {
            xSmoothed = Mathf.Lerp(xSmoothed, speed, smoothing * Time.deltaTime);
            sprite.flipX = true;
        }
        else if (target.x < transform.position.x)
        {
            xSmoothed = Mathf.Lerp(xSmoothed, speed * -1, smoothing * Time.deltaTime);
            sprite.flipX = false;
        }
    }

    private IEnumerator SpawnZ()
    {
        yield return new WaitForSeconds(3);

        Instantiate(zPrefab, zSpawnPos.position, Quaternion.identity);

        StartCoroutine("SpawnZ");
    }

    private bool LineOfSight(Vector2 start, Vector2 end, LayerMask obstacleMask)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, obstacleMask);
        return hit.collider == null;
    }
}