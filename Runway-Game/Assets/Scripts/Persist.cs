using UnityEngine;

// Class to ensure background music continues playing on scene load
public class Persist : MonoBehaviour
{
    static Persist instance = null;
    public static Persist Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
