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

    public void Directory(TerminalData hubData, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    {
        int numPerCol = hubData.batteryCells.Count / 2;

        for (int i = 0; i < numPerCol; i++)
        {
            MoveUpLine(textBoxCol1, textBoxCol2);

            if (hubData.cellBatteryAmount[i] > 0)
            {
                textBoxCol1.text += "cell" + i + "  --  " + hubData.cellBatteryAmount[i] + " kW " + "  --  " + hubData.cellPowerDraw[i] + "kW-c" + "  --  " + hubData.batteryCells[i];
            }
            else
            {
                textBoxCol1.text += "cell" + i + "  --  " + "drained";
            }

            if (hubData.cellBatteryAmount[i + numPerCol] > 0)
            {
                textBoxCol2.text += "cell" + (i + numPerCol) + "  --  " + hubData.cellBatteryAmount[i + numPerCol] + " kW " + "  --  " + hubData.cellPowerDraw[i + numPerCol] + "kW-c" + "  --  " + hubData.batteryCells[i + numPerCol];
            }
            else
            {
                textBoxCol2.text += "cell" + (i + numPerCol) + "  --  " + "drained";
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
                                if (hubData.batteryCells[cellNumber] == "unconnected")
                                {
                                    if (hubData.cellBatteryAmount[cellNumber] > 0)
                                    {
                                        hubData.batteryCells[cellNumber] = "system " + systemNumber;
                                        return;
                                    }
                                    else
                                    {
                                        MoveUpLine(textBoxCol1, textBoxCol2);
                                        textBoxCol1.text += "this cell is drained";
                                        return;
                                    }
                                }
                                else
                                {
                                    MoveUpLine(textBoxCol1, textBoxCol2);
                                    textBoxCol1.text += "this system is already connected to this cell";
                                    return;
                                }
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

    //public void ClearFunction(string[] inputIndcFunc, TerminalData hubData, TerminalData terminalData, TMP_Text textBoxCol1, TMP_Text textBoxCol2)
    //{
    //    if (inputIndcFunc[1].Substring(0, 1) == "[" && inputIndcFunc[1].Substring(6, 1) == "]" && inputIndcFunc[1].Substring(1, 4) == "cell")
    //    {
    //        int cellNumber = int.Parse(inputIndcFunc[1].Substring(5, 1));

    //        if (cellNumber <= (hubData.batteryCells.Count - 1))
    //        {
    //            int sysToRemove = int.Parse(hubData.batteryCells[cellNumber].Substring(hubData.batteryCells[cellNumber].Length - 1, 1));

    //            if (hubData.batteryCells[cellNumber] != "unconnected")
    //            {
    //                hubData.batteryCells[cellNumber] = "unconnected";
    //                hubData.cellPowerDraw[cellNumber] = 0;

    //                terminalData.systemsOnline--;
    //            }
    //            else
    //            {
    //                MoveUpLine(textBoxCol1, textBoxCol2);
    //                textBoxCol1.text += "this cell is already clear";
    //            }
    //        }
    //        else
    //        {
    //            MoveUpLine(textBoxCol1, textBoxCol2);
    //            textBoxCol1.text += "cell " + cellNumber + " does not exist";
    //        }
    //    }
    //    else
    //    {
    //        MoveUpLine(textBoxCol1, textBoxCol2);
    //        textBoxCol1.text += "incorrectly refferencing a cell";
    //    }
    //}

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
            if (inputIndcFunc[2].Substring(0, 1) == "[" && inputIndcFunc[2].Substring(5, 1) == "]" && inputIndcFunc[2].Substring(1, 3) == "sys")
            {
                int sysNumber = int.Parse(inputIndcFunc[2].Substring(4, 1));

                if (int.Parse(terminalData.avaliableSystems[0].Substring(terminalData.avaliableSystems[0].Length - 1, 1)) == sysNumber)
                {
                    for (int i = 0; i < hubData.batteryCells.Count; i++)
                    {
                        if (hubData.batteryCells[i] != "unconnected")
                        {
                            Debug.Log("system num: " + sysNumber);
                            Debug.Log("battery cell sys num:" + int.Parse(hubData.batteryCells[i].Substring(hubData.batteryCells[i].Length - 1, 1)));

                            if (sysNumber == int.Parse(hubData.batteryCells[i].Substring(hubData.batteryCells[i].Length - 1, 1)))
                            {
                                if (hubData.cellPowerDraw[i] == 0)
                                {
                                    terminalData.systemsOnline++;
                                }

                                if (powerChange <= 5)
                                {
                                    hubData.cellPowerDraw[i] = powerChange;
                                    return;
                                }
                                else
                                {
                                    MoveUpLine(textBoxCol1, textBoxCol2);
                                    textBoxCol1.text += "warning: the power maximum is 5";
                                    MoveUpLine(textBoxCol1, textBoxCol2);
                                    textBoxCol1.text += "the power has been set to 5";

                                    hubData.cellPowerDraw[i] = 5;
                                    return;
                                }
                            }
                        }
                    }

                    MoveUpLine(textBoxCol1, textBoxCol2);
                    textBoxCol1.text += inputIndcFunc[1] + " cannot be connected to a battery cell";
                }
                else
                {
                    MoveUpLine(textBoxCol1, textBoxCol2);
                    textBoxCol1.text += "this system is not avaliable to this terminal";
                }
            }
            else
            {
                MoveUpLine(textBoxCol1, textBoxCol2);
                textBoxCol1.text += "incorrectly refferencing a system";
            }
        }
        else
        {
            MoveUpLine(textBoxCol1, textBoxCol2);
            textBoxCol1.text += "incorrectly refferencing power adjustment";
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
