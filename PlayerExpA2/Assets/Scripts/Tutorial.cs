using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TerminalData hubTerminalData;

    [SerializeField] List<TerminalInputControl> terminals;

    [SerializeField] List<Image> trm1Checkboxes = new List<Image>();
    [SerializeField] List<Image> trm2Checkboxes = new List<Image>();
    [SerializeField] List<Image> trm3Checkboxes = new List<Image>();

    [SerializeField] List<Sprite> checkBoxSprites = new List<Sprite>();


    int[] boxesChecked = { 0, 0, 0, 0, 0, 0 };
    int checkBoxesComplete;

    void Start()
    {

    }

    void Update()
    {
        CheckPowerDraw();
        CheckConnected();

        checkBoxesComplete = 0;

        foreach (int count in boxesChecked)
        {
            checkBoxesComplete += count;
        }

        if (checkBoxesComplete >= 6)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void CheckPowerDraw()
    {
        if (terminals[0].onlinePowerDraw > 0 && boxesChecked[1] != 1)
        {
            trm1Checkboxes[1].sprite = checkBoxSprites[1];
            boxesChecked[1] = 1;
        }

        if (terminals[1].onlinePowerDraw > 0 && boxesChecked[3] != 1)
        {
            trm2Checkboxes[1].sprite = checkBoxSprites[1];
            boxesChecked[3] = 1;
        }

        if (terminals[2].onlinePowerDraw > 0 && boxesChecked[5] != 1)
        {
            trm3Checkboxes[1].sprite = checkBoxSprites[1];
            boxesChecked[5] = 1;
        }
    }

    void CheckConnected()
    {
        for (int i = 0; i < hubTerminalData.batteryCells.Count; i++)
        {
            if (hubTerminalData.batteryCells[i] == "system 0" && boxesChecked[0] != 1)
            {
                trm1Checkboxes[0].sprite = checkBoxSprites[1];
                boxesChecked[0] = 1;
            }
            else if (hubTerminalData.batteryCells[i] == "system 1" && boxesChecked[2] != 1)
            {
                trm2Checkboxes[0].sprite = checkBoxSprites[1];
                boxesChecked[2] = 1;
            }
            else if (hubTerminalData.batteryCells[i] == "system 2" && boxesChecked[4] != 1)
            {
                trm3Checkboxes[0].sprite = checkBoxSprites[1];
                boxesChecked[4] = 1;
            }
        }
    }
}
