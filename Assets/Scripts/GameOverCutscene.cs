using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCutscene : MonoBehaviour
{
    public GameObject intro;

    public Animator fancyDudeAnimator;
    public Rigidbody2D fancyDudeRigidbody;

    public GameObject endingBubble;

    public Animator heliAnimator;
    public Transform heliTransform;

    private bool heliSpeedup;
    private bool heliFly;

    private float heliFlySpeed;

    public GameObject results;
    public TMP_Text finalScoreDisplay;
    public TMP_Text bestScoreDisplay;

    public GameObject skipButton;

    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip doorSound;
    public AudioClip bubbleSound;

    private void Awake()
    {
        intro.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine("Ending");

        if(PlayerPrefs.GetInt("endingSkip") == 1)
        {
            SkipCutscene();
        }
    }

    void Update()
    {
        if (heliSpeedup)
        {
            heliAnimator.speed += 0.3f * Time.deltaTime;
        }

        if (heliFly)
        {
            heliTransform.position = new Vector2(heliTransform.position.x, heliTransform.position.y + heliFlySpeed * Time.deltaTime);
            heliFlySpeed += 0.4f * Time.deltaTime;
        }

        finalScoreDisplay.text = "You climbed " + PlayerPrefs.GetInt("lastScore").ToString() + " meters";
        bestScoreDisplay.text = "Best: " + PlayerPrefs.GetInt("bestScore").ToString() + "m";

        if(PlayerPrefs.GetInt("lastScore") > PlayerPrefs.GetInt("bestScore"))
        {
            PlayerPrefs.SetInt("bestScore", PlayerPrefs.GetInt("lastScore"));
        }

        if (results.activeInHierarchy)
        {
            if (Input.GetButtonDown("Jump"))
            {
                ReturnToMenu();
            }
        }
    }

    private IEnumerator Ending()
    {
        yield return new WaitForSeconds(3);

        intro.SetActive(false);

        yield return new WaitForSeconds(2);

        fancyDudeAnimator.Play("FancyDudeBored");

        yield return new WaitForSeconds(2f);

        endingBubble.SetActive(true);
        audioSource.PlayOneShot(bubbleSound);

        yield return new WaitForSeconds(4.2f);

        fancyDudeAnimator.Play("FancyDudeIdle");

        yield return new WaitForSeconds(1.3f);

        fancyDudeAnimator.Play("FancyDudeJump");
        fancyDudeRigidbody.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);
        audioSource.PlayOneShot(jumpSound);

        yield return new WaitForSeconds(0.6f);

        fancyDudeAnimator.gameObject.SetActive(false);
        audioSource.PlayOneShot(doorSound);

        heliAnimator.Play("HeliFlying");
        heliAnimator.speed = 0;
        heliSpeedup = true;

        yield return new WaitForSeconds(1.8f);

        heliFly = true;

        yield return new WaitForSeconds(2f);

        skipButton.SetActive(false);
        results.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();

        fancyDudeAnimator.gameObject.SetActive(false);

        heliAnimator.Play("HeliFlying");
        heliAnimator.speed = 0;
        heliSpeedup = true;
        heliFly = true;
        intro.SetActive(false);
        endingBubble.SetActive(false);

        skipButton.SetActive(false);
        results.SetActive(true);
    }
}