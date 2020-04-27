using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderObject : MonoBehaviour
{
    public static SliderObject instance;

    public Slider slider;

    public TMP_InputField inputField;

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

    public bool CheckLegalRaise(int num)
    {
        int myChips = GameManager.players[Client.instance.myId].chipTotal;
        if (num > myChips)
        {
            return false;
        }
        return true;
    }

    public void SetSliderValue()
    {
        
        int toSet;
        bool legalInput = int.TryParse(inputField.text, out toSet);
        Debug.Log($"To set = {toSet} ");// int.parse = {int.Parse(value.text)}");
        int myChips = GameManager.players[Client.instance.myId].chipTotal;
        if (!legalInput)
        {
            return;
        }
        // cant bet more chips than you have and negative chips
        if (toSet > myChips)
        {
            toSet = myChips;
        }
        else if (toSet < 0)
        {
            toSet = 0;
        }
        slider.value = toSet;
        RoundSliderValue();
    }

    public void IncrementSlider()
    {
        slider.value += 5;
    }

    public void DecrementSlider()
    {
        slider.value -= 5;
    }



    public void RoundSliderValue()
    {
        slider.value = Mathf.RoundToInt(slider.value / 5) * 5;
    }

    public void UpdateValue()
    {
        RoundSliderValue();
        inputField.text = slider.value.ToString();
    }

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0;

        slider.maxValue = 1000;
    }

    // Update is called once per frame
    public static void UpdateMinMaxValues()
    {
        if (instance == null)
        {
            return;
        }
        instance.slider.minValue = 0;
        int chips = GameManager.players[Client.instance.myId].chipTotal;
        if (chips < 0)
        {
            chips = 0;
        }
        instance.slider.maxValue = chips;
    }

}
