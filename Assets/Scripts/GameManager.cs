using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainMenu;
    public GameObject cutscene;
    public GameObject gameplayUi;

    public Gradient skyGradient;
    private float time;

    public bool gameRunning = false;
    public bool firstCutscene = false;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public TMP_Text meters;

    public Toggle endingSkip;

    public SpriteRenderer controls;

    public int coins;
    public TMP_Text coinsDisplay;

    public TMP_Text bestDisplay;

    public GameObject callingAudio;

    public AudioSource introMusic;
    public AudioSource loopMusic;

    public bool playerInMusicSpeedupZone;

    public Transform lava;

    public float musicPitchDist;

    public GameObject menuMusic;

    private void Start()
    {
        instance = this;

        if (PlayerPrefs.GetInt("endingSkip") == 1)
        {
            endingSkip.isOn = true;
        }
        else
        {
            endingSkip.isOn = false;
        }
    }

    private void Update()
    {
        bestDisplay.text = "Best: " + PlayerPrefs.GetInt("bestScore").ToString() + "m";

        if(controls == null)
        {
            var controlss = GameObject.Find("controls");

            if(controlss != null)
            {
                controls = controlss.GetComponent<SpriteRenderer>();
            }
        }

        if (gameRunning)
        {
            time = Mathf.Lerp(time, 1, Time.deltaTime * 1);
            Camera.main.backgroundColor = skyGradient.Evaluate(time);

            if(PlayerController.instance.health == 3)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
            }
            else if (PlayerController.instance.health == 2)
            {
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(false);
            }
            else if (PlayerController.instance.health == 1)
            {
                heart1.SetActive(true);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }
            else if (PlayerController.instance.health == 0)
            {
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
            }

            if(controls != null)
            {
                controls.color = new Color(1, 1, 1, controls.color.a + 0.2f * Time.deltaTime);
            }
        }

        if (!PlayerController.instance.dead)
        {
            meters.text = Mathf.Clamp(Mathf.Round(PlayerController.instance.transform.position.y / 1.3f), 0, Mathf.Infinity).ToString() + "m";
        }

        if (endingSkip.isOn)
        {
            PlayerPrefs.SetInt("endingSkip", 1);
        }
        else
        {
            PlayerPrefs.SetInt("endingSkip", 0);
        }

        coinsDisplay.text = coins.ToString();

        if(!introMusic.isPlaying && introMusic.gameObject.activeInHierarchy)
        {
            introMusic.gameObject.SetActive(false);
            loopMusic.gameObject.SetActive(true);
        }

        float dist = Mathf.Abs(PlayerController.instance.transform.position.y - lava.position.y);

        //calculate pitch based on the inverse of distance
        float pitch = 1f + (musicPitchDist - dist) / 2;

        //ensure pitch is clamped between 1 and a higher value
        pitch = Mathf.Clamp(pitch, 1f, 1.7f);

        loopMusic.pitch = pitch;

        if(!gameRunning && !firstCutscene)
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartGame();
            }
        }
    }

    public void StartGame()
    {
        menuMusic.SetActive(false);
        if (PlayerPrefs.GetInt("endingSkip") == 1)
        {
            SkipCutscene();
        }
        else
        {
            firstCutscene = true;
            mainMenu.SetActive(false);
            cutscene.SetActive(true);

            PlayerController.instance.StartCoroutine("WakeUp");
            Phone.instance.animator.Play("PhoneCall");
            callingAudio.SetActive(true);
        }
    }

    public void SkipCutscene()
    {
        mainMenu.SetActive(false);
        cutscene.SetActive(false);

        Bed.instance.StopAllCoroutines();

        firstCutscene = false;
        gameRunning = true;

        introMusic.gameObject.SetActive(true);

        callingAudio.SetActive(false);
        PlayerController.instance.StopAllCoroutines();
        Phone.instance.animator.Play("PhoneIdle");
        PlayerController.instance.calling = false;
        PlayerController.instance.introbubble.SetActive(false);
        PlayerController.instance.StartCoroutine("WakeUp");
        gameplayUi.SetActive(true);
    }
}