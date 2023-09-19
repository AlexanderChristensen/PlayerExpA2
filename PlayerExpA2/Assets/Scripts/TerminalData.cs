using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalData : MonoBehaviour
{
    [SerializeField] MechanismManager mechanismManager;

    [Header ("in editor for debugging")]
    //for hub
    public List<string> batteryCells = new List<string>();
    public List<float> cellBatteryAmount = new List<float>();
    public List<float> cellPowerDraw = new List<float>();

    //for terminal
    public List<string> batteryCellsGiven = new List<string>();
    public List<string> cellSysConnection = new List<string>();    
    public List<float> cellPower = new List<float>();
    public List<string> unconnectedSystems = new List<string>();

    [Header("only needed for hub")]
    [SerializeField] int batteryCellCount;
    public List<GameObject> terminals = new List<GameObject>();
    [Header("only needed for terminal")]
    public int numberOfSystems;

    int cellIndex;

    void Start()
    {
        for (int i = 0; i < batteryCellCount; i++)
        {
            batteryCells.Add("unconnected");
            cellBatteryAmount.Add(mechanismManager.shipPowerTotal / batteryCellCount);
            cellPowerDraw.Add(0);
        }

        for (int i = 0; i < numberOfSystems; i ++)
        {
            unconnectedSystems.Add("system " + i);
        }
    }

    public void GiveCell(string cellName)
    {
        batteryCellsGiven.Add(cellName);
        cellSysConnection.Add("unconnected");
        cellPower.Add(0);
    }

    public void RemoveCell(string cellName)
    {
        for (int i = 0; i < batteryCellsGiven.Count; i++)
        {
            if (batteryCellsGiven[i] == cellName)
            {
                cellIndex = i;
                batteryCellsGiven.RemoveAt(i);
            }
        }

        if (cellSysConnection[cellIndex] != "unconnected")
        {
            unconnectedSystems.Add(cellSysConnection[cellIndex]);
            cellSysConnection[cellIndex] = "unconnected";

            cellPower.RemoveAt(cellIndex);
        }
    }
}
