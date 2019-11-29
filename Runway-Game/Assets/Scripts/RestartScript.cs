using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScript : MonoBehaviour {
	public void RestartGame()
    {
        SceneManager.LoadScene(2);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
