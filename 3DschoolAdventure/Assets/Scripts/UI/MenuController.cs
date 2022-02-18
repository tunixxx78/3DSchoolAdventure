using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu, pauseMenu, gameOver;
    //public GameObject settingsFirstButton, pauseFirstButton, startMenuFirstButton;
    private MenuStates menuState;

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
                ControlGameOver();
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
    }

    public void ControlPauseMenu(Transform transform)
    {
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
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(settingsFirstButton);

        //What happens from buttons
        switch (transform.name)
        {
            case "Continue":
                MenuState = MenuStates.RETURN;
                break;
        }
    }

    //Inactivates gameobjects from the UI
    public void ControlReturn()
    {
        if (settingsMenu.gameObject.activeSelf)
        {
            settingsMenu.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            Time.timeScale = 1;
        }
    }

    public void ControlGameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
}
