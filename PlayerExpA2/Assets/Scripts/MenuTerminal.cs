using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuTerminal : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text textBoxCol1;
    [SerializeField] TMP_Text textBoxCol2;

    void Start()
    {
        inputField.caretWidth = 20;

        inputField.ActivateInputField();
    }


    void Update()
    {
        inputField.ActivateInputField();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitText();
        }
    }

    public void SubmitText()
    {
        MoveUpLine();
        textBoxCol1.text += "> " + inputField.text;

        SearchForFunction(inputField.text);

        inputField.text = null;
    }

    void MoveUpLine()
    {
        textBoxCol1.text += "\n";
        textBoxCol2.text += "\n";
    }

    void SearchForFunction(string inputFunction)
    {
        string[] inputIndcFunc = inputFunction.Split(' ');

        if (inputIndcFunc[0] == "ls")
        {
            if (inputIndcFunc.Length > 1)
            {
                MoveUpLine();
                textBoxCol1.text += "ls is all required to list directory";
            }
            else
            {
                textBoxCol1.text += "\ngame\ntraining-module\nsettings\nquit\n";
                textBoxCol1.text += ".......................................\n~ type run followed by the program you wish to run";
            }
        }
        else if (inputIndcFunc[0] == "run")
        {
            if (inputIndcFunc[1] == "game")
            {
                SceneManager.LoadScene("Game");
            }
            else if (inputIndcFunc[1] == "training-module")
            {
                textBoxCol1.text += "\ntheres no trianing scene just yet, run game instead";
            }
            else if (inputIndcFunc[1] == "settings")
            {
                textBoxCol1.text += "\nthere is no settings menu yet (there may never be)";
            }
            else if (inputIndcFunc[1] == "quit")
            {
                Application.Quit();
            }
            else
            {
                textBoxCol1.text += "\n" + inputIndcFunc[1] + " is not a program that can be run";
            }
        }
        else
        {
            textBoxCol1.text += "\nthe term" + inputIndcFunc[0] + " is not a recognised";
        }
    }

}
