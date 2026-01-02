using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Rigidbody2D rb;

    private float xSmoothed;

    [Header("Movement Values")]
    public float speed;
    public float smoothing;
    public float jumpForce;

    private float xInput;

    public bool slipping;

    [Header("Ground Check")]
    public bool grounded;

    [Header("Animations")]
    public Animator animator;
    public SpriteRenderer sprite;
    public bool calling;

    public GameObject introbubble;

    [Header("Health")]
    public int health = 3;
    private bool invincible = false;
    public bool damageKnockback;
    public GameObject damageFx;
    public SpriteRenderer damageFxSprite;
    public bool dead;
    public Vector2 deadCamPos;
    public GameObject shieldVisual;
    public bool shield;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip damageSound;
    public AudioClip jumpSound;
    public AudioClip slipSound;
    public AudioClip hpupSound;
    public AudioClip coinSound;
    public AudioClip bubbleSound;

    private float collectCooldown;

    private void Start()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();

        transform.parent = null;

        sprite.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if (!dead)
        {
            Movement();
        }
        else
        {
            xInput = 0;
            xSmoothed = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        Animations();

        if(health <= 0)
        {
            Die();
        }

        if (shield)
        {
            shieldVisual.SetActive(true);
        }
        else
        {
            shieldVisual.SetActive(false);
        }

        if(collectCooldown > 0)
        {
            collectCooldown -= 1 * Time.deltaTime;
        }

        if(collectCooldown < 0)
        {
            collectCooldown = 0;
        }
    }

    private void Movement()
    {
        if (GameManager.instance.gameRunning)
        {
            if (!damageKnockback)
            {
                xInput = Input.GetAxisRaw("Horizontal");
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (grounded)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    audioSource.PlayOneShot(jumpSound);
                }
            }
        }

        if (!slipping)
        {
            xSmoothed = Mathf.Lerp(xSmoothed, xInput * speed, smoothing * Time.deltaTime);
        }
        else
        {
            xSmoothed = Mathf.Lerp(xSmoothed, xInput * speed, (smoothing / 3.5f) * Time.deltaTime);
        }

        if (!damageKnockback)
        {
            rb.linearVelocity = new Vector2(xSmoothed, rb.linearVelocity.y);
        }
    }

    private void Animations()
    {
        if (!dead)
        {
            if (grounded && xInput == 0 && !calling)
            {
                animator.Play("PlayerIdle");
            }

            if (grounded && xInput != 0)
            {
                animator.Play("PlayerWalk");
                animator.speed = Mathf.Abs(rb.linearVelocity.x) / 3.5f;
            }
            else
            {
                animator.speed = 1;
            }

            if (!grounded && rb.linearVelocity.y >= 0 && !damageKnockback)
            {
                animator.Play("PlayerJump");
            }

            if (!grounded && rb.linearVelocity.y < 0 && !damageKnockback)
            {
                animator.Play("PlayerFall");
            }

            if (xInput < 0)
            {
                sprite.flipX = true;
            }

            if (xInput > 0)
            {
                sprite.flipX = false;
            }

            if (calling)
            {
                animator.Play("PlayerPhone");
            }

            if (damageKnockback)
            {
                animator.Play("PlayerHurt");
            }
        }
        else
        {
            animator.Play("PlayerHurt");
        }
    }

    public IEnumerator WakeUp()
    {
        if (GameManager.instance.firstCutscene)
        {
            yield return new WaitForSeconds(2);
        }

        sprite.color = new Color(1, 1, 1, 1);
        if (GameManager.instance.firstCutscene)
        {
            audioSource.PlayOneShot(jumpSound);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        Bed.instance.sprite.sprite = Bed.instance.emptyBed;

        if (GameManager.instance.firstCutscene)
        {
            StartCoroutine("Walk");
        }
    }

    public IEnumerator Walk()
    {
        yield return new WaitForSeconds(2);

        xInput = 1;

        yield return new WaitForSeconds(0.35f);

        xInput = 0;

        yield return new WaitForSeconds(0.5f);

        calling = true;
        Phone.instance.animator.Play("PhonePickedUp");
        GameManager.instance.callingAudio.SetActive(false);
        GameManager.instance.introMusic.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        introbubble.SetActive(true);
        audioSource.PlayOneShot(bubbleSound);

        yield return new WaitForSeconds(9.5f);

        calling = false;
        Phone.instance.animator.Play("PhoneIdle");

        GameManager.instance.firstCutscene = false;
        GameManager.instance.gameRunning = true;
        GameManager.instance.cutscene.SetActive(false);
        GameManager.instance.gameplayUi.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lethal"))
        {
            if (!invincible && !dead)
            {
                invincible = true;
                damageKnockback = true;
                damageFx.SetActive(true);

                audioSource.PlayOneShot(damageSound);

                if(collision.gameObject.transform.position.x <= transform.position.x)
                {
                    Debug.Log("Knock Right");
                    rb.linearVelocity = new Vector2(0, 0);

                    xInput = 0;
                    xSmoothed = 0;

                    rb.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                }
                else if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    Debug.Log("Knock Left");

                    rb.linearVelocity = new Vector2(0, 0);

                    xInput = 0;
                    xSmoothed = 0;

                    rb.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                }

                if (!shield)
                {
                    health--;
                }
                else
                {
                    shield = false;
                }

                StartCoroutine("InvincibilityFlash");
            }
        }

        if (collision.gameObject.CompareTag("Instakill"))
        {
            if (!dead)
            {
                health = 0;
                audioSource.PlayOneShot(damageSound);
            }
        }

        if (collision.gameObject.CompareTag("BananaPeel"))
        {
            if (!dead)
            {
                damageKnockback = true;

                audioSource.PlayOneShot(slipSound);

                if (collision.gameObject.transform.GetChild(0).position.x >= transform.position.x)
                {
                    Debug.Log("Slip Right");
                    rb.linearVelocity = new Vector2(0, 0);

                    xInput = 0;
                    xSmoothed = 0;

                    rb.AddForce(Vector2.right * 7, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                }
                else if (collision.gameObject.transform.GetChild(0).position.x < transform.position.x)
                {
                    Debug.Log("Slip Left");

                    rb.linearVelocity = new Vector2(0, 0);

                    xInput = 0;
                    xSmoothed = 0;

                    rb.AddForce(Vector2.left * 7, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                }

                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            if (!dead && collectCooldown == 0)
            {
                Destroy(collision.gameObject);
                GameManager.instance.coins++;
                audioSource.PlayOneShot(coinSound);
                collectCooldown = 0.1f;
            }
        }

        if (collision.gameObject.CompareTag("HeartPickup"))
        {
            if (!dead && health < 3 && collectCooldown == 0)
            {
                Destroy(collision.gameObject);
                health++;
                audioSource.PlayOneShot(hpupSound);
                collectCooldown = 0.1f;
            }
        }

        if (collision.gameObject.CompareTag("ShieldPickup"))
        {
            if (!dead && !shield && collectCooldown == 0)
            {
                Destroy(collision.gameObject);
                shield = true;
                audioSource.PlayOneShot(hpupSound);
                collectCooldown = 0.1f;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lethal"))
        {
            if (!invincible && !dead)
            {
                invincible = true;
                damageKnockback = true;
                damageFx.SetActive(true);

                audioSource.PlayOneShot(damageSound);

                if (collision.gameObject.transform.position.x <= transform.position.x)
                {
                    Debug.Log("Knock Right");
                    rb.linearVelocity = new Vector2(0, 0);

                    xInput = 0;
                    xSmoothed = 0;

                    rb.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                }
                else if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    Debug.Log("Knock Left");

                    rb.linearVelocity = new Vector2(0, 0);

                    xInput = 0;
                    xSmoothed = 0;

                    rb.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                }

                if (!shield)
                {
                    health--;
                }
                else
                {
                    shield = false;
                }

                StartCoroutine("InvincibilityFlash");
            }
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        //this code SUCKS.

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 0);
        damageFxSprite.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(0.05f);

        sprite.color = new Color(1, 1, 1, 1);
        damageFxSprite.color = new Color(1, 1, 1, 1);

        invincible = false;
    }

    public void Die()
    {
        if(!dead)
        {
            dead = true;
            deadCamPos = transform.position;
            GetComponent<BoxCollider2D>().enabled = false;
            sprite.sortingOrder = 30;
            rb.AddForce(Vector2.up * 6, ForceMode2D.Impulse);

            var score = Mathf.Clamp(Mathf.Round(transform.position.y / 1.3f), 0, Mathf.Infinity);

            PlayerPrefs.SetInt("lastScore", (int)score);

            GameManager.instance.loopMusic.Stop();

            StartCoroutine("GameOver");
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(1);
    }
}