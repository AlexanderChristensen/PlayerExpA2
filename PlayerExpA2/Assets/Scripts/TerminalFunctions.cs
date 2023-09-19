using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalFunctions
{
    public void CellDirectory(TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        int numPerCol = terminalData.batteryCells.Count / 2;

        for (int i = 0; i < numPerCol; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);

            if (terminalData.cellBatteryAmount[i] > 0)
            {
                textBoxCol1.text += "cell " + i + "   --   " + terminalData.batteryCells[i];
            }
            else
            {
                textBoxCol1.text += "cell " + i + "   --   " + "drained";
            }

            if (terminalData.cellBatteryAmount[i + numPerCol] > 0)
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + terminalData.batteryCells[i + numPerCol];
            }
            else
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + "drained";
            }
        }

        MoveUpLine(textBoxCol1, textBoxCol2);
        textBoxCol1.text += ".......................................\nterminals online:";
        textBoxCol2.text += "\n";

        for (int i = 0; i < terminalData.terminals.Count; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += terminalData.terminals[i].name;
        }

        textBoxCol2.text += "\n";
    }

    public void PowerDirectory(TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        int numPerCol = terminalData.batteryCells.Count / 2;

        for (int i = 0; i < numPerCol; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);

            if (terminalData.cellBatteryAmount[i] > 0)
            {
                textBoxCol1.text += "cell " + i + "   --   " + terminalData.cellBatteryAmount[i] + " kW " + "   --   " + terminalData.cellPowerDraw[i] + "kW-c";
            }
            else
            {
                textBoxCol1.text += "cell " + i + "   --   " + "drained";
            }

            if (terminalData.cellBatteryAmount[i + numPerCol] > 0)
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + terminalData.cellBatteryAmount[i + numPerCol] + " kW " + "   --   " + terminalData.cellPowerDraw[i + numPerCol] + "kW-c";
            }
            else
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + "drained";
            }
        }

        MoveUpLine(textBoxCol1, textBoxCol2);
        textBoxCol1.text += ".......................................\nterminals online:";
        textBoxCol2.text += "\n";

        for (int i = 0; i < terminalData.terminals.Count; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += terminalData.terminals[i].name + "   --   " + terminalData.terminals[i].gameObject.GetComponent<TerminalInputControl>().onlinePowerDraw + " kW-c";
        }

        textBoxCol2.text += "\n";
    }

    public void LinkFunction(string[] inputIndcFunc, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        if (inputIndcFunc[1].Substring(0, 3) == "trm")
        {
            int terminalNumber = int.Parse(inputIndcFunc[1].Substring(inputIndcFunc[1].Length - 1, 1));
            int cellNumber = int.Parse(inputIndcFunc[2].Substring(4, 1));

            if (inputIndcFunc[2].Substring(0, 1) == "[" && inputIndcFunc[2].Substring(5, 1) == "]" && inputIndcFunc[2].Substring(1, 3) == "cll")
            {
                if (terminalNumber <= terminalData.terminals.Count)
                {
                    if (cellNumber <= terminalData.batteryCells.Count)
                    {
                        terminalData.batteryCells[cellNumber] = "terminal " + terminalNumber;
                        terminalData.terminals[terminalNumber-1].GetComponent<TerminalData>().GiveCell("cell " + cellNumber);
                    }
                    else
                    {
                        MoveUpLine(textBoxCol1, textBoxCol2);
                        textBoxCol1.text += "cell " + cellNumber + " does not exist";
                    }
                }
                else
                {
                    MoveUpLine(textBoxCol1, textBoxCol2);
                    textBoxCol1.text += "terminal " + terminalNumber + " does not exist";
                }
            }
            else
            {
                MoveUpLine(textBoxCol1, textBoxCol2);
                textBoxCol1.text += "incorrectly refferencing a cell";
            }
        }
        else
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += inputIndcFunc[1] + " cannot be connected to a battery cell";
        }
    }

    public void ClearFunction(string[] inputIndcFunc, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        if (inputIndcFunc[1].Substring(0, 1) == "[" && inputIndcFunc[1].Substring(5, 1) == "]" && inputIndcFunc[1].Substring(1, 3) == "cll")
        {
            int cellNumber = int.Parse(inputIndcFunc[1].Substring(4, 1));
            if (cellNumber <= terminalData.batteryCells.Count)
            {
                int terminalToRemove = int.Parse(terminalData.batteryCells[cellNumber].Substring(terminalData.batteryCells[cellNumber].Length - 1, 1));

                terminalData.terminals[terminalToRemove - 1].GetComponent<TerminalData>().RemoveCell("cell " + cellNumber);
                terminalData.batteryCells[cellNumber] = "unconnected";

                Debug.Log("Terminal to remove: " + terminalToRemove);
            }
            else
            {
                MoveUpLine(textBoxCol1, textBoxCol2);
                textBoxCol1.text += "cell " + cellNumber + " does not exist";
            }
        }
        else
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += "incorrectly refferencing a cell";
        }
    }

    public void MoveUpLine(TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        textBoxCol1.text += "\n";
        textBoxCol2.text += "\n";
    }

    public void AdjustFunction(string[] inputIndcFunc, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    { 
        int powerChange;

        if (int.TryParse(inputIndcFunc[1], out powerChange))
        {
            int cellNumber = int.Parse(inputIndcFunc[2].Substring(4, 1));

            if (inputIndcFunc[2].Substring(0, 1) == "[" && inputIndcFunc[2].Substring(5, 1) == "]" && inputIndcFunc[2].Substring(1, 3) == "cll")
            {
                for (int i = 0; i < terminalData.batteryCellsGiven.Count; i++)
                {
                    if (int.Parse(terminalData.batteryCellsGiven[i].Substring(terminalData.batteryCellsGiven[i].Length - 1, 1)) == cellNumber)
                    {
                        if (terminalData.cellSysConnection[i] != "unconnected")
                        {
                            terminalData.cellPower[i] = powerChange;
                            return;
                        }
                        else
                        {
                            MoveUpLine(textBoxCol1, textBoxCol2);
                            textBoxCol1.text += "cell " + cellNumber + " does not have a system attached";
                        }
                    }
                }
            }
            else
            {
                MoveUpLine(textBoxCol1, textBoxCol2);
                textBoxCol1.text += "incorrectly refferencing a cell";
            }
        }
        else
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += inputIndcFunc[1] + " cannot be connected to a battery cell";
        }
    }

    public void LinkSystemFunction(string[] inputIndcFunc, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        if (inputIndcFunc[1].Substring(0, 3) == "sys")
        {
            int systemNumber = int.Parse(inputIndcFunc[1].Substring(inputIndcFunc[1].Length - 1, 1));
            int cellNumber = int.Parse(inputIndcFunc[2].Substring(4, 1));

            if (inputIndcFunc[2].Substring(0, 1) == "[" && inputIndcFunc[2].Substring(5, 1) == "]" && inputIndcFunc[2].Substring(1, 3) == "cll")
            {
                if (systemNumber <= terminalData.numberOfSystems)
                {
                    for (int i = 0; i < terminalData.batteryCellsGiven.Count; i++)
                    {
                        int cellsGivenNum = int.Parse(terminalData.batteryCellsGiven[i].Substring(terminalData.batteryCellsGiven[i].Length - 1, 1));
                        if (cellNumber == cellsGivenNum)
                        {
                            for (int o = 0; o < terminalData.batteryCellsGiven.Count; o++)
                            {
                                if (int.Parse(terminalData.batteryCellsGiven[o].Substring(terminalData.batteryCellsGiven[o].Length - 1, 1)) == cellNumber)
                                {
                                    terminalData.cellSysConnection[o] = "system " + systemNumber;
                                    terminalData.unconnectedSystems.Remove("system " + systemNumber);
                                    return;
                                }
                            }
                        }
                    }
                    MoveUpLine(textBoxCol1, textBoxCol2);

                    textBoxCol1.text += "cell " + cellNumber + " is not connected to this terminal";
                }
                else
                {
                    MoveUpLine(textBoxCol1, textBoxCol2);
                    textBoxCol1.text += "system " + systemNumber + " does not exist";
                }
            }
            else
            {
                MoveUpLine(textBoxCol1, textBoxCol2);
                textBoxCol1.text += "incorrectly referencing a cell";
            }
        }
        else
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += inputIndcFunc[1] + " cannot be connected to a battery cell";
        }


    }
}
