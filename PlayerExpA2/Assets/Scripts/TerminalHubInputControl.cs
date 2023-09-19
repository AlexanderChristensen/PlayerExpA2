using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalHubInputControl : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text textBoxCol1;
    [SerializeField] TMP_Text textBoxCol2;

    [SerializeField] string terminalName;

    TerminalData terminalData;
    TerminalInteraction terminalInteraction;

    TerminalFunctions terminalFunctions = new TerminalFunctions();

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

        CheckIfCellEmpty();
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
        else if(inputIndcFunc[0] == "exit")
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
                    terminalFunctions.CellDirectory(terminalData, textBoxCol1, textBoxCol2);
                }
                else if (inputIndcFunc[1] == "powerdir")
                {
                    terminalFunctions.PowerDirectory(terminalData, textBoxCol1, textBoxCol2);
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
            terminalFunctions.LinkFunction(inputIndcFunc, terminalData, textBoxCol1, textBoxCol2);
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

    void MoveUpLine()
    {
        textBoxCol1.text += "\n";
        textBoxCol2.text += "\n";
    }

    void CheckIfCellEmpty()
    {
        for (int i = 0; i < terminalData.cellBatteryAmount.Count; i++)
        {
            if (terminalData.cellBatteryAmount[i] <= 0)
            {
                if (terminalData.batteryCells[i] != "unconnected")
                {
                    int terminalToRemove = int.Parse(terminalData.batteryCells[i].Substring(terminalData.batteryCells[i].Length - 1, 1));

                    terminalData.terminals[terminalToRemove - 1].GetComponent<TerminalData>().RemoveCell("cell " + i);
                    terminalData.batteryCells[i] = "unconnected";

                    terminalData.cellPowerDraw[i] = 0;
                    terminalData.cellBatteryAmount[i] = 0;
                }
            }
        }
    }
}
