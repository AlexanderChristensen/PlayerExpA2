using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubScreen : MonoBehaviour
{
    TerminalFunctions terminalFunctions = new TerminalFunctions();

    [SerializeField] TMP_Text textBoxCol1;
    [SerializeField] TMP_Text textBoxCol2;

    TerminalData terminalData;

    void Start()
    {
        terminalData = GetComponent<TerminalData>();

        terminalFunctions.PowerDirectory(terminalData, textBoxCol1, textBoxCol2);
    }

    public void UpdateHubDisplay()
    {
        textBoxCol1.text = "";
        textBoxCol2.text = "";

        terminalFunctions.PowerDirectory(terminalData, textBoxCol1, textBoxCol2);
    }
}
