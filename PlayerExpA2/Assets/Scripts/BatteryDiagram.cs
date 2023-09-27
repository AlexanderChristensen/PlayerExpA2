using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BatteryDiagram : MonoBehaviour
{
    [SerializeField] TMP_Text maxText;
    [SerializeField] TMP_Text currentText;

    [SerializeField] TerminalData hubTerminal;
    [SerializeField] int cellNum;

    SliderController slider;

    bool setStartValues;

    void Start()
    {
        slider = GetComponent<SliderController>();
    }

    void Update()
    {
        if (!setStartValues)
        {
            slider.SetMaxValue(hubTerminal.cellBatteryAmount[cellNum], hubTerminal.cellBatteryAmount[cellNum]);
            maxText.text = hubTerminal.cellBatteryAmount[cellNum] + "kW";

            setStartValues = true;
        }

        slider.UpdateSlider(hubTerminal.cellBatteryAmount[cellNum]);
        currentText.text = hubTerminal.cellBatteryAmount[cellNum] + "kW";
    }
}
