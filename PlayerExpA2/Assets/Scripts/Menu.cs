using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] bool isMenu;

    [SerializeField] GameObject menuOverlay;

    bool gamePaused;

    void Start()
    {
        if (!isMenu)
        {
            menuOverlay.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    void Update()
    {
        if (!isMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                if (!gamePaused)
                {
                    menuOverlay.SetActive(true);
                    Time.timeScale = 0f;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;


                    gamePaused = true;
                }
                else
                {
                    menuOverlay.SetActive(false);
                    Time.timeScale = 1f;

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    gamePaused = false;
                }
            }
        }
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gamePaused = false;

        menuOverlay.SetActive(false);
    }

    public void LoadMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();

        if (!isMenu)
        {
            menuOverlay.SetActive(false);
        }
    }
}
