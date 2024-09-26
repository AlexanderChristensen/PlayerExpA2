using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnctedandDraw : MonoBehaviour
{
    [SerializeField] TerminalData terminalData;
    [SerializeField] TerminalInputControl terminalInput;

    [SerializeField] List<Sprite> dotSprites = new List<Sprite>();

    [SerializeField] string systemName;
    [SerializeField] Image connectImage;
    [SerializeField] TMP_Text connectText;

    [SerializeField] Image powerImage;
    [SerializeField] TMP_Text powerText;

    void Update()
    {

        ConnectCheck();

        if (terminalInput.onlinePowerDraw > 0)
        {
            powerImage.sprite = dotSprites[1];
            powerText.text = "connected";
        }
        else
        {
            powerImage.sprite = dotSprites[0];
            powerText.text = "unconnected";
        }
        
    }

    void ConnectCheck()
    {
        bool isConnected = false;

        for (int i = 0; i < terminalData.batteryCells.Count; i++)
        {
            if(terminalData.batteryCells[i] == systemName)
            {
                isConnected = true;
            }
        }


        if (isConnected)
        {
            connectImage.sprite = dotSprites[1];
            connectText.text = "connected";
        }
        else
        {
            connectImage.sprite = dotSprites[0];
            connectText.text = "unconnected";
        }
    }
}
