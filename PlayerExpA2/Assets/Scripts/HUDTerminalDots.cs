using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTerminalDots : MonoBehaviour
{
    [SerializeField] MechanismManager mechanismManager;
    [SerializeField] TerminalData hubTerminal;

    [SerializeField] List<TerminalData> terminal = new List<TerminalData>();

    [SerializeField] List<Sprite> dot1Sprites = new List<Sprite>();
    [SerializeField] List<Sprite> dot2Sprites = new List<Sprite>();
    [SerializeField] List<Sprite> dot3Sprites = new List<Sprite>();

    [SerializeField] Image dot1Image;
    [SerializeField] Image dot2Image;
    [SerializeField] Image dot3Image;

    void Update()
    {
        if (mechanismManager.oxygenFilteringDraw > 0)
        {

            if (hubTerminal.cellBatteryAmount[terminal[0].activeCells[0]] < (mechanismManager.shipPowerTotal / hubTerminal.batteryCellCount) / 2)
            {
                dot1Image.sprite = dot1Sprites[2];
            }
            else
            {
                dot1Image.sprite = dot1Sprites[1];
            }

        }
        else
        {
            dot1Image.sprite = dot1Sprites[0];
        }

        if (mechanismManager.sheildDraw > 0)
        {
            dot2Image.sprite = dot2Sprites[1];
        }
        else
        {
            dot2Image.sprite = dot2Sprites[0];
        }

        if (mechanismManager.experimentDraw > 0)
        {
            dot3Image.sprite = dot3Sprites[1];
        }
        else
        {
            dot3Image.sprite = dot3Sprites[0];
        }
    }
}
