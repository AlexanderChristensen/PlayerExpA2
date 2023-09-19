using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMechanismManager : MonoBehaviour
{
    [SerializeField] string staticUnits;
    [SerializeField] string cycleUnits;

    [SerializeField] TMP_Text maxValueText;
    [SerializeField] TMP_Text currentValueText;
    [SerializeField] TMP_Text cycleValueText;

    SliderController slider;

    void Start()
    {
        slider = GetComponent<SliderController>();
    }

    public void SetStartValues(float max, float start, float cycleChange)
    {
        slider.SetMaxValue(max, start);

        maxValueText.text = max + staticUnits;
        currentValueText.text = start + staticUnits;
        cycleValueText.text = cycleChange + cycleUnits;
    }

    public void UpdateValues(float current, float cycleCurrent)
    {
        slider.UpdateSlider(current);

        currentValueText.text = current + staticUnits;
        cycleValueText.text = cycleCurrent + cycleUnits;
    }
}
