using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateBatteryNumber : MonoBehaviour
{
    [SerializeField] MechanismManager mechanismManager;
    [SerializeField] TMP_Text maxBattery;
    [SerializeField] TMP_Text currentBattery;

    bool setStartingValues;

    void Update()
    {
        if (!setStartingValues)
        {
            maxBattery.text = mechanismManager.shipPowerTotal + "kW";
        }

        currentBattery.text = mechanismManager.shipPower + "kW";
    }
}
