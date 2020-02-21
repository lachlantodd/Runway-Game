using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private AudioSource music;
    private int tiltStatus;
    public Text tiltButtonText;
    public SpriteRenderer black;

    private void Awake()
    {
        if (GameObject.FindWithTag("music") != null)
            music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        if (music != null && !music.isPlaying && PlayerPrefs.GetInt("music", 1) == 1)
        {
            music.Play();
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
            PlayerPrefs.SetInt("music", 0);
        }
        else if (music != null)
        {
            music.Play();
            PlayerPrefs.SetInt("music", 1);
        }
        PlayerPrefs.Save();
    }

    public void OpenHelpMenu()
    {
        SceneManager.LoadScene(4);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OpenLevels()
    {
        //StartCoroutine("Fader");
        SceneManager.LoadScene(1);
    }

    public void OpenSkins()
    {
        SceneManager.LoadScene(2);
    }

    public void SelectLevel1()
    {
        SceneManager.LoadScene(3);
        PlayerPrefs.SetInt("level", 1);
    }

    public void SelectLevel2()
    {
        SceneManager.LoadScene(3);
        PlayerPrefs.SetInt("level", 2);
    }

    public void SelectLevel3()
    {
        SceneManager.LoadScene(3);
        PlayerPrefs.SetInt("level", 3);
    }

    private IEnumerator Fader()
    {
        //anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(1);
    }
}
