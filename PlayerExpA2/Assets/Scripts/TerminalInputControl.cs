using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalInputControl : MonoBehaviour
{
    public float onlinePowerDraw;

    [SerializeField] TerminalData hubTerminal;

    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text textBoxCol1;
    [SerializeField] TMP_Text textBoxCol2;

    [SerializeField] string terminalName;

    List<string> terminalSystems = new List<string>();

    TerminalData terminalData;

    TerminalInteraction terminalInteraction;

    TerminalFunctions terminalFunctions = new TerminalFunctions();

    float totalPowerDraw;
    float tempTotalPowerDraw;

    private void Start()
    {
        terminalData = GetComponent<TerminalData>();

        terminalInteraction = GetComponent<TerminalInteraction>();

        inputField.caretWidth = 20;

        inputField.ActivateInputField();
    }

    void Update()
    {
        inputField.ActivateInputField();

        if (Input.GetKeyDown(KeyCode.Return) && terminalInteraction.interacting)
        {
            SubmitText();
        }

        CheckIfOnline();

        UpdateHubCellDraw();
    }


    public void SubmitText()
    {
        MoveUpLine();
        textBoxCol1.text += "> " + inputField.text;

        SearchForFunction(inputField.text);

        inputField.text = null;
    }

    void SearchForFunction(string inputFunction)
    {
        string[] inputIndcFunc = inputFunction.Split(' ');

        if (inputIndcFunc[0] == "help")
        {
            textBoxCol1.text += "\nls\ncelldir\npowerdir\n";
            textBoxCol2.text += "\nadjst\nclr\nlink\nexit\n";
        }
        else if (inputIndcFunc[0] == "exit")
        {
            terminalInteraction.Exit();
        }
        else if (inputIndcFunc[0] == "ls")
        {
            if (!(inputIndcFunc.Length > 1))
            {
                MoveUpLine();
                textBoxCol1.text += "type in a directory name to list";
            }
            else
            {
                if (inputIndcFunc[1] == "celldir")
                {
                    CellDirectory();
                }
                else if (inputIndcFunc[1] == "powerdir")
                {
                    PowerDirectory();
                }
                else
                {
                    MoveUpLine();
                    textBoxCol1.text += "this directory does not exist";
                }
            }
        }
        else if (inputIndcFunc[0] == "clr")
        {
            terminalFunctions.ClearFunction(inputIndcFunc, terminalData, textBoxCol1, textBoxCol2);
        }
        else if (inputIndcFunc[0] == "link")
        {
            terminalFunctions.LinkSystemFunction(inputIndcFunc, terminalData, textBoxCol1, textBoxCol2);
        }
        else if (inputIndcFunc[0] == "adjst")
        {
            terminalFunctions.AdjustFunction(inputIndcFunc, terminalData, textBoxCol1, textBoxCol2);
        }
        else
        {
            if (inputFunction != "")
            {
                MoveUpLine();
                textBoxCol1.text += "The term" + " '" + inputFunction + "' " + "is not recognized try typing help for avaliable functions";
            }
        }
    }

    public void CellDirectory()
    {
        if (terminalData.batteryCellsGiven.Count > 4)
        {
            int numPerCol = terminalData.batteryCellsGiven.Count / 2;

            for (int i = 0; i < numPerCol; i++)
            {
                MoveUpLine();
                textBoxCol1.text += terminalData.batteryCellsGiven[i] + "   --   " + terminalData.cellSysConnection[i];
                textBoxCol2.text += terminalData.batteryCellsGiven[i + numPerCol] + "   --   " + terminalData.cellSysConnection[i + numPerCol];
            }
        }
        else
        {
            for (int i = 0; i < terminalData.batteryCellsGiven.Count; i++)
            {
                MoveUpLine();
                textBoxCol1.text += terminalData.batteryCellsGiven[i] + "   --   " + terminalData.cellSysConnection[i];
            }
        }

        MoveUpLine();
        textBoxCol1.text += ".......................................\nunassigned systems:";
        textBoxCol2.text += "\n";

        for (int i = 0; i < terminalData.unconnectedSystems.Count; i++)
        {
            MoveUpLine();
            textBoxCol1.text += terminalData.unconnectedSystems[i];
        }
    }

    public void PowerDirectory()
    {
        if (terminalData.batteryCellsGiven.Count > 4)
        {
            int numPerCol = terminalData.batteryCellsGiven.Count / 2;

            for (int i = 0; i < numPerCol; i++)
            {
                MoveUpLine();
                textBoxCol1.text += terminalData.batteryCellsGiven[i] + "   --   " + terminalData.cellPower[i] + " kW-c";
                textBoxCol2.text += terminalData.batteryCellsGiven[i + numPerCol] + "   --   " + terminalData.cellPower[i + numPerCol] + " kW-c";
            }
        }
        else
        {
            for (int i = 0; i < terminalData.batteryCellsGiven.Count; i++)
            {
                MoveUpLine();
                textBoxCol1.text += terminalData.batteryCellsGiven[i] + "   --   " + terminalData.cellPower[i] + " kW-c";
            }
        }

        MoveUpLine();
        textBoxCol1.text += ".......................................\ntotal terminal power draw: " + totalPowerDraw + " kW-c";
        textBoxCol2.text += "\n";
    }

    void MoveUpLine()
    {
        textBoxCol1.text += "\n";
        textBoxCol2.text += "\n";
    }

    void CheckIfOnline()
    {
        tempTotalPowerDraw = 0;

        for (int i = 0; i < terminalData.cellPower.Count; i++)
        {
            tempTotalPowerDraw += terminalData.cellPower[i];
        }

        totalPowerDraw = tempTotalPowerDraw;

        for (int i = 0; i < terminalData.cellPower.Count; i++)
        {
            if (terminalData.cellPower[i] == 0)
            {
                return;
            }

            onlinePowerDraw = totalPowerDraw;
        }

        if (terminalData.cellPower.Count == 0) 
        {
            onlinePowerDraw = 0;
        }
    }

    void UpdateHubCellDraw()
    {
        if (terminalData.cellSysConnection.Count > 0 && terminalData.batteryCellsGiven.Count > 0) 
        {
            for (int i = 0; i < terminalData.cellPower.Count; i++)
            {
                int index = int.Parse(terminalData.batteryCellsGiven[i].Substring(5, 1));

                Debug.Log(index);

                if (index <= hubTerminal.cellPowerDraw.Count && index > -1)
                {
                    hubTerminal.cellPowerDraw[index] = terminalData.cellPower[i];
                }
            }
        }
    }
}
