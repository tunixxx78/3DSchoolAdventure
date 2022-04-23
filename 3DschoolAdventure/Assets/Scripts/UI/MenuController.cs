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
    public bool tryAgain = false, win = false, lose = false, finalLevel = false, introSkipped = false;
    public TMP_Text resultText, finalPointsText, gameOverText;
    public string resultStringLose, resultStringWin, resultStringFinished, gameOverStringLose, gameOverStringWin, gameOverStringFinished;
    public GameObject yesButton, noButton, quitButton, startOverButton, continueButton/*, dialogue*/;
    //public GameObject[] dialoguePages;
    //public int currentPage = 0;
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
        //introSkipped = PlayerPrefs.GetInt("IntroSkipped") == 1 ? true : false;
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

        //if (introSkipped && currentLevel == 2)
        //{
        //    dialogue.SetActive(true);
        //    for (int i = 0; i < dialoguePages.Length; i++)
        //    {
        //        if (currentPage < dialoguePages.Length)
        //        {
        //            dialoguePages[currentPage].SetActive(true);
        //            if (currentPage > 0)
        //            {
        //                dialoguePages[currentPage - 1].SetActive(false);
        //            }
        //        }
        //        else
        //        {
        //            dialogue.SetActive(false);
        //        }
        //    }
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
            case MenuStates.CUTSCENE:
                ControlCutscene(transform);
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
        }

        //What happens from buttons
        switch (transform.name)
        {
            case "Play":
                MenuState = MenuStates.CUTSCENE;
                break;
            case "Settings":
                MenuState = MenuStates.SETTINGS;
                break;
            case "Exit":
                Application.Quit();
                break;
        }
    }

    public void ControlCutscene(Transform transform)
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("IntroAnimation"))
        {
            SceneManager.LoadScene("IntroAnimation");
        }
        switch (transform.name)
        {
            case "Skip":
                MenuState = MenuStates.CHARACTERSELECTION;
                introSkipped = true;
                PlayerPrefs.SetInt("IntroSkipped", introSkipped ? 1 : 0);
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
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(currentLevel))
        {
            SceneManager.LoadScene(currentLevel);
        }
        //switch (transform.name)
        //{
        //    case "Next":
        //        currentPage++;
        //        break;
        //}
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
                resultText.text = resultStringWin;
                gameOverText.text = gameOverStringWin;
                yesButton.SetActive(false);
                noButton.SetActive(false);
                quitButton.SetActive(true);
                startOverButton.SetActive(false);
                continueButton.SetActive(true);
            }
            else
            {
                resultText.text = resultStringFinished;
                gameOverText.text = gameOverStringFinished;
                yesButton.SetActive(false);
                noButton.SetActive(false);
                quitButton.SetActive(true);
                startOverButton.SetActive(true);
                continueButton.SetActive(false);
            }
        }
        if (lose)
        {
            resultText.text = resultStringLose;
            gameOverText.text = gameOverStringLose;
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
