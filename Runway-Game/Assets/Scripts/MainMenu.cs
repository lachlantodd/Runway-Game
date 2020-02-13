using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    AudioSource music;

    public void Awake()
    {
        music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        if (music != null && !music.isPlaying &&
            PlayerPrefs.GetInt("music", 1) == 1)
        {
            music.Play();
        }
    }

    public void MuteMusic()
    {
        if (music.isPlaying)
        {
            music.Stop();
            PlayerPrefs.SetInt("music", 0);
        }
        else
        {
            music.Play();
            PlayerPrefs.SetInt("music", 1);
        }
        PlayerPrefs.Save();
    }

    public void ExitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(3);
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(1);
    }
    public void SelectLevel1()
    {
        SceneManager.LoadScene(2);
        PlayerPrefs.SetInt("level", 1);
    }
    public void SelectLevel2()
    {
        SceneManager.LoadScene(2);
        PlayerPrefs.SetInt("level", 2);
    }
    public void SelectLevel3()
    {
        SceneManager.LoadScene(2);
        PlayerPrefs.SetInt("level", 3);
    }
}
