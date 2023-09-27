using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] GrappleMovement movement;
    [SerializeField] MechanismManager mechanismManager;
    [SerializeField] SliderController velocitySlider;
    [SerializeField] TMP_Text velocityLastCycleText;

    void Update()
    {
        velocitySlider.SetMaxValue(mechanismManager.lastSampleVelocity + (mechanismManager.velocityAllowance*2), movement.velocity);
        velocitySlider.SetMinValue(mechanismManager.lastSampleVelocity - (mechanismManager.velocityAllowance*2), movement.velocity);

        velocitySlider.UpdateSlider(movement.velocity);

        velocityLastCycleText.text = mechanismManager.lastSampleVelocity.ToString("F2") + "m/s";
    }
}
