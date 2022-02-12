using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu, pauseMenu;
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

    //Calls a method when the menuState value changes
    public void ChangeState()
    {
        switch (menuState)
        {
            case MenuStates.STARTMENU:
                ControlStartMenu(transform);
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
        }
    }

    public void ControlStartMenu(Transform transform)
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(2))
        {
            SceneManager.LoadScene(2);
        }

        //What happens from buttons
        switch (transform.name)
        {
            case "Play":
                MenuState = MenuStates.GAMEVIEW;
                break;
            case "Settings":
                MenuState = MenuStates.SETTINGS;
                break;
            case "Exit":
                Application.Quit();
                break;
        }
    }

    public void ControlGameView(Transform transform)
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(1))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void ControlPauseMenu(Transform transform)
    {
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
        }

        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
