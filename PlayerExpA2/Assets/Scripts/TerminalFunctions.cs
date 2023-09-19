using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerminalFunctions
{
    public void CellDirectory(TerminalData hubData, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        int numPerCol = hubData.batteryCells.Count / 2;

        for (int i = 0; i < numPerCol; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);

            if (hubData.cellBatteryAmount[i] > 0)
            {
                textBoxCol1.text += "cell " + i + "   --   " + hubData.batteryCells[i];
            }
            else
            {
                textBoxCol1.text += "cell " + i + "   --   " + "drained";
            }

            if (hubData.cellBatteryAmount[i + numPerCol] > 0)
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + hubData.batteryCells[i + numPerCol];
            }
            else
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + "drained";
            }
        }

        MoveUpLine(textBoxCol1, textBoxCol2);
        textBoxCol1.text += ".......................................\nsystems avaliable:";
        textBoxCol2.text += "\n";

        for (int i = 0; i < terminalData.avaliableSystems.Count; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += terminalData.avaliableSystems[i];
        }

        textBoxCol2.text += "\n";
    }

    public void PowerDirectory(TerminalData hubData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        int numPerCol = hubData.batteryCells.Count / 2;

        for (int i = 0; i < numPerCol; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);

            if (hubData.cellBatteryAmount[i] > 0)
            {
                textBoxCol1.text += "cell " + i + "   --   " + hubData.cellBatteryAmount[i] + " kW " + "   --   " + hubData.cellPowerDraw[i] + "kW-c";
            }
            else
            {
                textBoxCol1.text += "cell " + i + "   --   " + "drained";
            }

            if (hubData.cellBatteryAmount[i + numPerCol] > 0)
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + hubData.cellBatteryAmount[i + numPerCol] + " kW " + "   --   " + hubData.cellPowerDraw[i + numPerCol] + "kW-c";
            }
            else
            {
                textBoxCol2.text += "cell " + (i + numPerCol) + "   --   " + "drained";
            }
        }

        MoveUpLine(textBoxCol1, textBoxCol2);
        textBoxCol1.text += ".......................................\nsystems avaliable:";
        textBoxCol2.text += "\n";

        for (int i = 0; i < hubData.terminals.Count; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += hubData.terminals[i].name + "   --   " + hubData.terminals[i].gameObject.GetComponent<TerminalInputControl>().onlinePowerDraw + " kW-c";
        }

        textBoxCol2.text += "\n";
    }

    public void LinkFunction(string[] inputIndcFunc, TerminalData hubData, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        if (inputIndcFunc[1].Substring(0, 3) == "sys")
        {
            int systemNumber = int.Parse(inputIndcFunc[1].Substring(inputIndcFunc[1].Length - 1, 1));
            int cellNumber = int.Parse(inputIndcFunc[2].Substring(5, 1));

            if (inputIndcFunc[2].Substring(0, 1) == "[" && inputIndcFunc[2].Substring(6, 1) == "]" && inputIndcFunc[2].Substring(1, 4) == "cell")
            {
                if (systemNumber - 1 <= hubData.systems.Count)
                {
                    for (int i = 0; i < terminalData.avaliableSystems.Count; i ++)
                    {
                        if (int.Parse(terminalData.avaliableSystems[i].Substring(terminalData.avaliableSystems[i].Length - 1, 1)) == systemNumber)
                        {
                            if (cellNumber <= hubData.batteryCells.Count)
                            {
                                hubData.batteryCells[cellNumber] = "system " + systemNumber;
                                return;
                            }
                            else
                            {
                                MoveUpLine(textBoxCol1, textBoxCol2);
                                textBoxCol1.text += "cell " + cellNumber + " does not exist";
                                return;
                            }
                        }
                    }
                    MoveUpLine(textBoxCol1, textBoxCol2);
                    textBoxCol1.text += "this system is not avaliable to this terminal";
                }
                else
                {
                    MoveUpLine(textBoxCol1, textBoxCol2);
                    textBoxCol1.text += "terminal " + systemNumber + " does not exist";
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
        if (inputIndcFunc[1].Substring(0, 1) == "[" && inputIndcFunc[1].Substring(6, 1) == "]" && inputIndcFunc[1].Substring(1, 4) == "cell")
        {
            int cellNumber = int.Parse(inputIndcFunc[1].Substring(5, 1));

            if (cellNumber <= (terminalData.batteryCells.Count - 1))
            {
                int sysToRemove = int.Parse(terminalData.batteryCells[cellNumber].Substring(terminalData.batteryCells[cellNumber].Length - 1, 1));

                terminalData.batteryCells[cellNumber] = "unconnected";
                terminalData.cellPowerDraw[cellNumber] = 0; 
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

    

    public void AdjustFunction(string[] inputIndcFunc, TerminalData hubData, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    { 
        int powerChange;

        if (int.TryParse(inputIndcFunc[1], out powerChange))
        {
            if (inputIndcFunc[2].Substring(0, 1) == "[" && inputIndcFunc[2].Substring(6, 1) == "]" && inputIndcFunc[2].Substring(1, 4) == "cell")
            {
                int cellNumber = int.Parse(inputIndcFunc[2].Substring(5, 1));
                if ((hubData.batteryCells.Count - 1) >= cellNumber)
                {
                    if (hubData.batteryCells[cellNumber] != "unconnected")
                    {
                        for (int i = 0; i < terminalData.avaliableSystems.Count; i++)
                        {
                            if (int.Parse(terminalData.avaliableSystems[i].Substring(terminalData.avaliableSystems[i].Length - 1, 1)) == int.Parse(hubData.batteryCells[cellNumber].Substring(hubData.batteryCells[cellNumber].Length - 1, 1)))
                            {
                                hubData.cellPowerDraw[cellNumber] = powerChange;
                                return;
                            }
                        }
                        MoveUpLine(textBoxCol1, textBoxCol2);
                        textBoxCol1.text += "this system is not avaliable to this terminal";
                    }
                    else
                    {
                        MoveUpLine(textBoxCol1, textBoxCol2);
                        textBoxCol1.text += "cell " + cellNumber + " does not have a system attached";
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

    //public void LinkSystemFunction(string[] inputIndcFunc, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    //{
    //    if (inputIndcFunc[1].Substring(0, 3) == "sys")
    //    {
    //        int systemNumber = int.Parse(inputIndcFunc[1].Substring(inputIndcFunc[1].Length - 1, 1));
    //        int cellNumber = int.Parse(inputIndcFunc[2].Substring(4, 1));

    //        if (inputIndcFunc[2].Substring(0, 1) == "[" && inputIndcFunc[2].Substring(5, 1) == "]" && inputIndcFunc[2].Substring(1, 3) == "cll")
    //        {
    //            if (systemNumber <= terminalData.numberOfSystems)
    //            {
    //                for (int i = 0; i < terminalData.batteryCellsGiven.Count; i++)
    //                {
    //                    int cellsGivenNum = int.Parse(terminalData.batteryCellsGiven[i].Substring(terminalData.batteryCellsGiven[i].Length - 1, 1));
    //                    if (cellNumber == cellsGivenNum)
    //                    {
    //                        for (int o = 0; o < terminalData.batteryCellsGiven.Count; o++)
    //                        {
    //                            if (int.Parse(terminalData.batteryCellsGiven[o].Substring(terminalData.batteryCellsGiven[o].Length - 1, 1)) == cellNumber)
    //                            {
    //                                terminalData.cellSysConnection[o] = "system " + systemNumber;
    //                                terminalData.unconnectedSystems.Remove("system " + systemNumber);
    //                                return;
    //                            }
    //                        }
    //                    }
    //                }
    //                MoveUpLine(textBoxCol1, textBoxCol2);

    //                textBoxCol1.text += "cell " + cellNumber + " is not connected to this terminal";
    //            }
    //            else
    //            {
    //                MoveUpLine(textBoxCol1, textBoxCol2);
    //                textBoxCol1.text += "system " + systemNumber + " does not exist";
    //            }
    //        }
    //        else
    //        {
    //            MoveUpLine(textBoxCol1, textBoxCol2);
    //            textBoxCol1.text += "incorrectly referencing a cell";
    //        }
    //    }
    //    else
    //    {
    //        MoveUpLine(textBoxCol1, textBoxCol2);
    //        textBoxCol1.text += inputIndcFunc[1] + " cannot be connected to a battery cell";
    //    }


    //}
}
