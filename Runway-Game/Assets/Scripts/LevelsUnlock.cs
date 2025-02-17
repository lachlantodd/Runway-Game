﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelsUnlock : MonoBehaviour
{
    private int totalLandings1, totalLandings2, totalLandings3;
    public Button carrierButton, spaceButton;
    public GameObject carrierText, carrierCount, spaceText, spaceCount;

    private void Start()
    {
        CheckLandingsCount();
    }

    private void CheckLandingsCount()
    {
        totalLandings1 = PlayerPrefs.GetInt("landings1", 0);
        totalLandings2 = PlayerPrefs.GetInt("landings2", 0);
        if (totalLandings1 >= 3)
        {
            carrierButton.interactable = true;
            carrierText.SetActive(false);
            carrierCount.SetActive(false);
        }
        else
        {
            carrierButton.interactable = false;
            carrierText.SetActive(true);
            carrierCount.SetActive(true);
            carrierCount.GetComponent<TextMeshProUGUI>().text = "(" + (3 - totalLandings1) + " left)";
        }
        if (totalLandings2 >= 3)
        {
            spaceButton.interactable = true;
            spaceText.SetActive(false);
            spaceCount.SetActive(false);
        }
        else
        {
            spaceButton.interactable = false;
            spaceText.SetActive(true);
            spaceCount.SetActive(true);
            spaceCount.GetComponent<TextMeshProUGUI>().text = "(" + (3 - totalLandings2) + " left)";
        }
    }
}
