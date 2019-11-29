using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {
    public void SelectPlaneWhite()
    {
        PlayerPrefs.SetInt("planeType", 0);
        SceneManager.LoadScene(0);
    }
    public void SelectPlaneBlack()
    {
        PlayerPrefs.SetInt("planeType", 1);
        SceneManager.LoadScene(0);
    }
    public void SelectShuttle()
    {
        PlayerPrefs.SetInt("planeType", 2);
        SceneManager.LoadScene(0);
    }
}
