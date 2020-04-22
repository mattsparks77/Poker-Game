using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderObject : MonoBehaviour
{
    public static SliderObject instance;

    public Slider slider;
    public TextMeshProUGUI value;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
    }

    public void UpdateValue()
    {
       // slider.value = slider.value*5;
        value.text = "$"+ slider.value.ToString();
    }

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0;

        slider.maxValue = 1000;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public static void UpdateMinMaxValues()
    {
        instance.slider.minValue = 0;
        int chips = GameManager.players[Client.instance.myId].chipTotal;
        if (chips < 0)
        {
            chips = 0;
        }
        instance.slider.maxValue = chips;
    }

}
