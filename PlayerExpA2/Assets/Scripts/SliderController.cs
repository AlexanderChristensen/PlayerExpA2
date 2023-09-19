using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void SetMaxValue(float maximumValue, float currentValue)
    {
        slider.maxValue = maximumValue;
        slider.value = currentValue;
    }

    public void UpdateSlider(float currentValue)
    {
        slider.value = currentValue;
    }
}
