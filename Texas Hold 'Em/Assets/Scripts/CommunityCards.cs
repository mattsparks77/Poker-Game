using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommunityCards : MonoBehaviour
{
    public static CommunityCards instance;
    public static Transform[] placeholders = new Transform[5];
    public Image[] communityImagesUI = new Image[5];
    public static int nextEmptySpot = 0;

    public GameObject communityCardsUIObject;
    public GameObject communityCardsAlternateUIObject;
    public Transform originalPosition;
    public Sprite cardBack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    /// <summary>
    /// Sets card ui to the card back.
    /// </summary>
    public void ResetCardUI()
    {
        foreach (Image i in communityImagesUI)
        {
            i.sprite = cardBack;
        }
    }

    public void Initialize()
    {
        placeholders[0] = this.transform.GetChild(0);
        placeholders[1] = this.transform.GetChild(1);
        placeholders[2] = this.transform.GetChild(2);
        placeholders[3] = this.transform.GetChild(3);
        placeholders[4] = this.transform.GetChild(4);
   

    }
}
