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
                textBoxCol1.text += "\ngame\ntraining-module\nintro-text\nquit\nsettings\n";
                textBoxCol1.text += ".......................................\n!type     run     followed by the program you wish to run";
            }
        }
        else if (inputIndcFunc[0] == "run")
        {
            if (inputIndcFunc.Length > 1)
            {

                if (inputIndcFunc[1] == "game")
                {
                    if (PlayerPrefs.GetInt("playedTutorial") > 0)
                    {
                        SceneManager.LoadScene("Game");
                    }
                    else
                    {
                        SceneManager.LoadScene("IntroText");
                    }

                    return;
                }
                else if (inputIndcFunc[1] == "training-module")
                {
                    SceneManager.LoadScene("tutorial");
                    return;
                }
                else if (inputIndcFunc[1] == "intro-text")
                {
                    SceneManager.LoadScene("IntroText");
                    return;
                }
                else if (inputIndcFunc[1] == "settings")
                {
                    textBoxCol1.text += "\nthere is no settings menu yet (there may never be)";
                    return;
                }
                else if (inputIndcFunc[1] == "quit")
                {
                    Application.Quit();
                    return;
                }
                else
                {

                    textBoxCol1.text += "\n" + inputIndcFunc[1] + " is not a program that can be run";
                    return;
                }
            }
            else
            {
                textBoxCol1.text += "\nyou must follow run with a program to run";
            }
        }
        else if(inputIndcFunc[0] == "resetprefs")
        {
            PlayerPrefs.SetInt("playedTutorial", 0);
        }
        else
        {
            textBoxCol1.text += "\nthe term " + inputIndcFunc[0] + " is not a recognised";
        }
    }

}
