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
    [SerializeField] TMP_Text cycleDrainText;
    [SerializeField] TMP_Text cycleRegenText;

    SliderController slider;

    void Start()
    {
        slider = GetComponent<SliderController>();
    }

    public void SetStartValues(float max, float start, float cycleDrain, float cycleRegen)
    {
        slider.SetMaxValue(max, start);

        maxValueText.text = max + staticUnits;
        currentValueText.text = start + staticUnits;
        cycleDrainText.text = cycleDrain + cycleUnits;
        cycleRegenText.text = cycleRegen + cycleUnits;
    }

    public void UpdateValues(float current, float cycleDrain, float cycleRegen)
    {
        slider.UpdateSlider(current);

        currentValueText.text = current + staticUnits;
        cycleDrainText.text = "-" + cycleDrain + cycleUnits;
        cycleRegenText.text = "+" + cycleRegen + cycleUnits;
    }
}
