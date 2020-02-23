using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScript : MonoBehaviour
{
    private int scene;
    public Animator anim;
    public Image black;

    public void RestartLevel()
    {
        scene = 3;
        StartCoroutine("Fader");
    }

    public void ReturnToMenu()
    {
        scene = 0;
        StartCoroutine("Fader");
    }

    private IEnumerator Fader()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(scene);
    }
}
