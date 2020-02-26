using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private AudioSource music;
    public Text muteText;
    private int tiltStatus;
    public Text tiltButtonText;
    public Image black;
    public Animator anim;
    private int scene;

    private void Awake()
    {
        if (GameObject.FindWithTag("music") != null)
            music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        if (music != null && !music.isPlaying && PlayerPrefs.GetInt("music", 1) == 1)
        {
            music.Play();
            muteText.text = "Mute";
        }
        else if (muteText != null && PlayerPrefs.GetInt("music", 1) == 0)
        {
            muteText.text = "Unmute";
        }
        if (tiltButtonText != null)
        {
            tiltStatus = PlayerPrefs.GetInt("tiltEnabled", 0);
            if (tiltStatus == 0)
                tiltButtonText.text = "Tilt Controls: Disabled";
            else
                tiltButtonText.text = "Tilt Controls: Enabled";
        }
    }

    public void ToggleTilt()
    {
        tiltStatus = PlayerPrefs.GetInt("tiltEnabled", 0);
        if (tiltStatus == 0)
        {
            PlayerPrefs.SetInt("tiltEnabled", 1);
            tiltButtonText.text = "Tilt Controls: Enabled";
        }
        else
        {
            PlayerPrefs.SetInt("tiltEnabled", 0);
            tiltButtonText.text = "Tilt Controls: Disabled";
        }
    }

    public void MuteMusic()
    {
        if (music != null && music.isPlaying)
        {
            music.Stop();
            muteText.text = "Unmute";
            PlayerPrefs.SetInt("music", 0);
        }
        else if (music != null)
        {
            music.Play();
            muteText.text = "Mute";
            PlayerPrefs.SetInt("music", 1);
        }
        PlayerPrefs.Save();
    }

    public void OpenHelpMenu()
    {
        scene = 4;
        StartCoroutine("Fader");
    }

    public void OpenMainMenu()
    {
        scene = 0;
        StartCoroutine("Fader");
    }

    public void ExitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OpenLevels()
    {
        scene = 1;
        StartCoroutine("Fader");
    }

    public void OpenSkins()
    {
        scene = 2;
        StartCoroutine("Fader");
    }

    public void SelectLevel1()
    {
        scene = 3;
        StartCoroutine("Fader");
        PlayerPrefs.SetInt("level", 1);
    }

    public void SelectLevel2()
    {

        scene = 3;
        StartCoroutine("Fader");
        PlayerPrefs.SetInt("level", 2);
    }

    public void SelectLevel3()
    {

        scene = 3;
        StartCoroutine("Fader");
        PlayerPrefs.SetInt("level", 3);
    }

    private IEnumerator Fader()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(scene);
    }
}
