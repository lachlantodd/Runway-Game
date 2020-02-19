using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public GameObject backgroundObject;
    public GameObject runwayObject;
    private int level;
    private Sprite background;
    private Sprite runway;
    public Sprite backgroundLevel1;
    public Sprite backgroundLevel2;
    public Sprite backgroundLevel3;
    public Sprite runwayLevel1;
    public Sprite runwayLevel2;
    public Sprite runwayLevel3;

    void Start()
    {
        level = PlayerPrefs.GetInt("level", 1);
        switch (level)
        {
            default:
            case 1:
                background = backgroundLevel1;
                runway = runwayLevel1;
                break;
            case 2:
                background = backgroundLevel2;
                runway = runwayLevel2;
                break;
            case 3:
                background = backgroundLevel3;
                runway = runwayLevel3;
                break;
        }
        backgroundObject.GetComponent<SpriteRenderer>().sprite = background;
        runwayObject.GetComponent<SpriteRenderer>().sprite = runway;
    }
}
