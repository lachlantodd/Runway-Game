using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SkinsMenu : MonoBehaviour
{

    private static int planeType1, planeType2, planeType3;
    private static int tenPointLandingslvl1, tenPointLandingslvl2, tenPointLandingslvl3;
    public TextMeshProUGUI planeText, jetText, shuttleText;
    public GameObject planeTextObj, jetTextObj, shuttleTextObj;
    public GameObject planeWhite, planeBlack, jetGrey, jetBlack, shuttleWhite, shuttleBlack;

    private void Start()
    {
        planeType1 = PlayerPrefs.GetInt("plane1", 0);
        planeType2 = PlayerPrefs.GetInt("plane2", 0);
        planeType3 = PlayerPrefs.GetInt("plane3", 0);
        if (planeType1 == 0)
        {
            planeWhite.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
            planeBlack.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        }
        else
        {
            planeWhite.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            planeBlack.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        }
        if (planeType2 == 0)
        {
            jetGrey.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
            jetBlack.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        }
        else
        {
            jetGrey.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            jetBlack.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        }
        if (planeType3 == 0)
        {
            shuttleWhite.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
            shuttleBlack.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        }
        else
        {
            shuttleWhite.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            shuttleBlack.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        }

        tenPointLandingslvl1 = PlayerPrefs.GetInt("10PointLandings1", 0);
        tenPointLandingslvl2 = PlayerPrefs.GetInt("10PointLandings2", 0);
        tenPointLandingslvl3 = PlayerPrefs.GetInt("10PointLandings3", 0);
        if (3 - tenPointLandingslvl1 > 0)
        {
            planeText.text = "(" + (3 - tenPointLandingslvl1) + " left)";
            planeTextObj.SetActive(true);
            planeBlack.GetComponent<Button>().interactable = false;
        }
        else
        {
            planeText.text = "";
            planeTextObj.SetActive(false);
            planeBlack.GetComponent<Button>().interactable = true;
        }
        if (2 - tenPointLandingslvl2 > 0)
        {
            jetText.text = "(" + (2 - tenPointLandingslvl2) + " left)";
            jetTextObj.SetActive(true);
            jetBlack.GetComponent<Button>().interactable = false;
        }
        else
        {
            jetText.text = "";
            jetTextObj.SetActive(false);
            jetBlack.GetComponent<Button>().interactable = true;
        }
        if (1 - tenPointLandingslvl3 > 0)
        {
            shuttleText.text = "(" + (1 - tenPointLandingslvl3) + " left)";
            shuttleTextObj.SetActive(true);
            shuttleBlack.GetComponent<Button>().interactable = false;
        }
        else
        {
            shuttleText.text = "";
            shuttleTextObj.SetActive(false);
            shuttleBlack.GetComponent<Button>().interactable = true;
        }
    }

    public void SelectPlaneWhite()
    {
        PlayerPrefs.SetInt("plane1", 0);
        planeWhite.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        planeBlack.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
    }

    public void SelectPlaneBlack()
    {
        PlayerPrefs.SetInt("plane1", 1);
        planeWhite.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        planeBlack.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
    }

    public void SelectJetGrey()
    {
        jetGrey.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        jetBlack.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        PlayerPrefs.SetInt("plane2", 0);
    }

    public void SelectJetBlack()
    {
        PlayerPrefs.SetInt("plane2", 1);
        jetGrey.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        jetBlack.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
    }

    public void SelectShuttleWhite()
    {
        shuttleWhite.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        shuttleBlack.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        PlayerPrefs.SetInt("plane3", 0);
    }

    public void SelectShuttleBlack()
    {
        shuttleWhite.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        shuttleBlack.GetComponent<RectTransform>().localScale = new Vector2(1.3f, 1.3f);
        PlayerPrefs.SetInt("plane3", 1);
    }
}
