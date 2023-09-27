using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubScreen : MonoBehaviour
{
    TerminalData terminalData;

    void Start()
    {
        terminalData = GetComponent<TerminalData>();

        //terminalFunctions.PowerDirectory(terminalData, textBoxCol1, textBoxCol2);
    }

    public void UpdateHubDisplay()
    {

    }
}
