using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScript : MonoBehaviour 
{
    private int level;

    private void Start()
    {
        level = PlayerPrefs.GetInt("level", 1);
    }

	public void RestartLevel()
    {
        SceneManager.LoadScene(3);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
