using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    private int level;
    private Sprite background;
    public Sprite backgroundLevel1;
    public Sprite backgroundLevel2;
    public Sprite backgroundLevel3;

    void Start () {
        level = PlayerPrefs.GetInt("level", 1);
        switch (level)
        {
            case 2:
                background = backgroundLevel2;
                break;
            case 3:
                background = backgroundLevel3;
                break;
            case 1:
            default:
                background = backgroundLevel1;
                break;
        }
        GetComponent<SpriteRenderer>().sprite = background;
    }
}
