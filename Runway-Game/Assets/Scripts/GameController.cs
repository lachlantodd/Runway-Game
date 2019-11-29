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

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameStarted)
        {
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
