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
    private MenuStates menuState;
    public bool tryAgain = false, win = false, lose = false, finalLevel = false;
    public TMP_Text resultText, finalPointsText;
    public GameObject yesButton, noButton, quitButton, startOverButton, continueButton;
    public int currentLevel;
    public int maxLevel;

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

    }

    private void Update()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //Pause activates from Esc and returns from pressing Esc again
        if (Input.GetKeyDown(KeyCode.Escape) && MenuState != MenuStates.PAUSE && MenuState != MenuStates.GAMEOVER)
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

        //if (levels[currentLevel] == levels[levels.Length])
        //{
        //    finalLevel = true;
        //}
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
        //if(SceneManager.GetActiveScene().buildIndex != SceneManager.GetSceneByBuildIndex(currentLevel))
        //{
        //    SceneManager.LoadScene(currentLevel);

        //    Cursor.visible = false;
        //}
        SceneManager.LoadScene(currentLevel);
        if (tryAgain)
        {
            SceneManager.LoadScene(currentLevel);
            Cursor.visible = false;
        }
    }

    public void ControlPauseMenu(Transform transform)
    {
        settingsMenu.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
        Cursor.visible = true;
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
        Cursor.visible = true;
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
            Cursor.visible = false;
        }
    }

    public void ControlGameOver(Transform transform)
    {
        gameOver.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;

        if (win)
        {
            if (currentLevel != maxLevel)
            {
                resultText.text = "YOU HAVE REACHED THE END OF THIS LEVEL. CONTINUE TO THE NEXT LEVEL OR SAVE & QUIT?";
                yesButton.SetActive(false);
                noButton.SetActive(false);
                quitButton.SetActive(true);
                startOverButton.SetActive(false);
                continueButton.SetActive(true);
            }
            else
            {
                resultText.text = "YOU HAVE REACHED THE FINISHLINE!";
                yesButton.SetActive(false);
                noButton.SetActive(false);
                quitButton.SetActive(true);
                startOverButton.SetActive(true);
                continueButton.SetActive(false);
            }
        }
        if (lose)
        {
            resultText.text = "YOU COULDN'T MAKE IT IN THIS LIFETIME. TRY AGAIN?";
            yesButton.SetActive(true);
            noButton.SetActive(true);
            quitButton.SetActive(false);
            startOverButton.SetActive(false);
            continueButton.SetActive(false);
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
            case "Start Over":
                tryAgain = true;
                MenuState = MenuStates.GAMEVIEW;
                break;
            case "Quit":
                MenuState = MenuStates.STARTMENU;
                break;
            case "Continue":
                currentLevel = currentLevel + 1;
                MenuState = MenuStates.GAMEVIEW;
                break;
        }
    }
}
