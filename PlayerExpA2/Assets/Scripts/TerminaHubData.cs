using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalHubData : MonoBehaviour
{
    public List<string> batteryCells = new List<string>();
    public List<float> cellBatteryAmount = new List<float>();

    [SerializeField] int batteryCellCount;

    [SerializeField] MechanismManager mechanismManager;
    public int numberOfTerminals;

    private void Start()
    {
        for (int i = 0; i < batteryCellCount; i ++)
        {
            batteryCells.Add("empty");
            cellBatteryAmount.Add(mechanismManager.shipPowerTotal/batteryCellCount);
        }
    }
}
