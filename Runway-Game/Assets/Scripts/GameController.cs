using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Transform planeTransform;
    public Text heightText;
    public Text countdownText;
    private int height;
    private bool gameStarted = false;
    private bool countdown = true;
    public GameObject countdownPage;
    public Rigidbody2D planeRigid;
    private int secondsToGo = 3;
    public GameObject background;
    AudioSource gameAudio, menuAudio;

    // Update is called once per frame

    private void Start()
    {
        menuAudio = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        if (menuAudio != null)
            menuAudio.Stop();
        gameAudio = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("music", 1) == 1)
        {
            gameAudio.volume = 1;
        }
        else
        {
            gameAudio.volume = 0;
        }
    }

    private void FixedUpdate()
    {
        if (gameStarted)
        {
            if (gameAudio.isPlaying == false)
                gameAudio.Play();
            if (countdownPage.activeSelf)
            {
                countdownPage.SetActive(false);
                planeRigid.simulated = true;
            }
            height = (int)planeTransform.position.z / 10;
            height = height * 10;
            if (height > 0)
            {
                height = 0;
            }
            height = height * 32;
            heightText.text = "Altitude: " + (-height).ToString() + " ft";
        }
        else if (countdown)
        {
            countdownPage.SetActive(true);
            planeRigid.simulated = false;
            StartCoroutine("CountdownTimer");
            countdown = false;
        }
    }

    public void MuteMusic()
    {
        if (gameAudio.volume == 1)
        {
            PlayerPrefs.SetInt("music", 0);
            gameAudio.volume = 0;
        } 
        else
        {
            PlayerPrefs.SetInt("music", 1);
            gameAudio.volume = 1;
        }
        PlayerPrefs.Save();
    }

    private IEnumerator CountdownTimer()
    {
        for (;;)
        {
            if (secondsToGo > 0)
            {
                countdownText.text = secondsToGo.ToString();
                secondsToGo -= 1;
                yield return new WaitForSeconds(1);
            }
            else
            {
                gameStarted = true;
                yield return null;
            }
        }
    }
}
