using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private AudioSource music;

    private void Awake()
    {
        if (GameObject.FindWithTag("music") != null)
            music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        if (music != null && !music.isPlaying && PlayerPrefs.GetInt("music", 1) == 1)
        {
            music.Play();
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
}
