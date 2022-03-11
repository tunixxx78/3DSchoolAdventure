using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu, pauseMenu, gameOver;
    //public GameObject settingsFirstButton, pauseFirstButton, startMenuFirstButton;
    private MenuStates menuState;
    public bool tryAgain = false, win = false, lose = false;
    public TMP_Text resultText, finalPointsText;
    public GameObject yesButton, noButton;

    public MenuStates MenuState
    {
        get { return menuState; }
        set
        {
            menuState = value;
            ChangeState();
        }
    }

    private void Start()
    {
        //EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        //Pause activates from Esc and returns from pressing Esc again
        if (Input.GetKeyDown(KeyCode.Escape) && MenuState != MenuStates.PAUSE)
        {
            Debug.Log("Escape pressed");
            MenuState = MenuStates.PAUSE;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && MenuState == MenuStates.PAUSE)
        {
            Debug.Log("Escape pressed");
            MenuState = MenuStates.RETURN;
        }

        if (win || lose)
        {
            MenuState = MenuStates.GAMEOVER;
        }
    }

    //Calls a method when the menuState value changes
    public void ChangeState()
    {
        switch (menuState)
        {
            case MenuStates.STARTMENU:
                ControlStartMenu(transform);
                break;
            case MenuStates.CHARACTERSELECTION:
                ControlCharacterSelection();
                break;
            case MenuStates.GAMEVIEW:
                ControlGameView(transform);
                break;
            case MenuStates.PAUSE:
                ControlPauseMenu(transform);
                break;
            case MenuStates.SETTINGS:
                ControlSettingsMenu(transform);
                break;
            case MenuStates.RETURN:
                ControlReturn();
                break;
            case MenuStates.GAMEOVER:
                ControlGameOver(transform);
                break;
        }
    }

    public void ControlStartMenu(Transform transform)
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("StartMenu"))
        {
            SceneManager.LoadScene("StartMenu");
            //EventSystem.current.SetSelectedGameObject(null);
            //EventSystem.current.SetSelectedGameObject(startMenuFirstButton);
        }

        //What happens from buttons
        switch (transform.name)
        {
            case "Play":
                MenuState = MenuStates.CHARACTERSELECTION;
                break;
            case "Settings":
                MenuState = MenuStates.SETTINGS;
                break;
            case "Exit":
                Application.Quit();
                break;
        }
    }

    public void ControlCharacterSelection()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("CharacterSelection"))
        {
            SceneManager.LoadScene("CharacterSelection");
        }
    }

    public void ControlGameView(Transform transform)
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("TuronTestScene"))
        {
            SceneManager.LoadScene("TuronTestScene");
        }
        if (tryAgain)
        {
            SceneManager.LoadScene("TuronTestScene");
        }
    }

    public void ControlPauseMenu(Transform transform)
    {
        settingsMenu.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        Time.timeScale = 0;

        //What happens from buttons
        switch (transform.name)
        {
            case "Continue":
                MenuState = MenuStates.RETURN;
                break;
            case "Settings":
                MenuState = MenuStates.SETTINGS;
                break;
            case "Try Again":
                tryAgain = true;
                MenuState = MenuStates.GAMEVIEW;
                break;
            case "Quit":
                MenuState = MenuStates.STARTMENU;
                break;
        }
    }

    public void ControlSettingsMenu(Transform transform)
    {
        settingsMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(settingsFirstButton);

        //What happens from buttons
        switch (transform.name)
        {
            case "Continue":
                MenuState = MenuStates.PAUSE;
                break;
        }
    }

    //Inactivates gameobjects from the UI
    public void ControlReturn()
    {
        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.gameObject.SetActive(false);
            //EventSystem.current.SetSelectedGameObject(null);
            Time.timeScale = 1;
        }
    }

    public void ControlGameOver(Transform transform)
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;

        if (win)
        {
            resultText.text = "YOU HAVE REACHED THE FINISHLINE!";
            yesButton.SetActive(false);
            noButton.SetActive(false);
        }
        if (lose)
        {
            resultText.text = "YOU COULDN'T MAKE IT IN THIS LIFETIME. TRY AGAIN?";
            yesButton.SetActive(true);
            noButton.SetActive(true);
        }

        switch (transform.name)
        {
            case "Yes":
                tryAgain = true;
                MenuState = MenuStates.GAMEVIEW;
                break;
            case "No":
                MenuState = MenuStates.STARTMENU;
                break;
        }
    }
}
