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
    public bool tryAgain = false, win = false, lose = false, finalLevel = false, dialogueActive = false;
    public TMP_Text resultText, finalPointsText, gameOverText;
    public string resultStringLose, resultStringWin, resultStringFinished, gameOverStringLose, gameOverStringWin, gameOverStringFinished;
    public GameObject yesButton, noButton, quitButton, continueButton, quitFinalButton, credits, scoreText, scoreOnGameView, dashText, hourGlass, dashSlider, timeOnGameView;
    public int currentLevel;
    public int maxLevel;
    private SoundFX sfx;

    public MenuStates MenuState
    {
        get { return menuState; }
        set
        {
            menuState = value;
            ChangeState();
        }
    }


    private void Awake()
    {
        sfx = FindObjectOfType<SoundFX>();
    }

    private void Update()
    {
        //currentLevel is the same as the activescene's buildIndex
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        //Pause activates from Esc
        if (Input.GetKeyDown(KeyCode.Escape) && MenuState != MenuStates.PAUSE && MenuState != MenuStates.GAMEOVER && dialogueActive == false)
        {
            Debug.Log("Escape pressed");
            MenuState = MenuStates.PAUSE;
        }

        //Win or lose -> GameOver state activates
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
            case MenuStates.HIGHSCORE:
                ControlHighScore();
                break;
        }
    }

    public void ControlStartMenu(Transform transform)
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("StartMenu"))
        {
            SceneManager.LoadScene("StartMenu");
            Cursor.visible = true;
        }

        //What happens from buttons
        switch (transform.name)
        {
            case "Play":
                StartCoroutine(WaitToChangeScene(MenuStates.CUTSCENE));
                break;
            case "Settings":
                MenuState = MenuStates.SETTINGS;
                break;
            case "Credits":
                credits.SetActive(true);
                break;
            case "Continue":
                credits.SetActive(false);
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
            StartCoroutine(WaitToChangeScene(MenuStates.CHARACTERSELECTION));
        }
        switch (transform.name)
        {
            case "Skip":
                //Turo added for skip howtoplay functionality
                SaveSystem.savingInstance.introIsSkipped = true;
                //For showing outFade if intro is skipped
                FindObjectOfType<IntroOutLogic>().ShowOutAnimation();

                StartCoroutine(WaitToChangeScene(MenuStates.CHARACTERSELECTION));
                break;
        }

    }

    public void ControlCharacterSelection()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("CharacterSelection"))
        {
            SceneManager.LoadScene("CharacterSelection");
        }
        // LAITETTAVA CHARACTERSELECTIONIN NAPPIIN - > StartCoroutine(WaitToChangeScene(MenuStates.GAMEVIEW));
    }

    public void ControlGameView(Transform transform)
    {
        Debug.Log(currentLevel);
        /*
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(currentLevel))
        {
            
            SceneManager.LoadScene(currentLevel);
            //StartCoroutine(SceneChangeFade(MenuStates.GAMEVIEW));
        }
        */
        if (tryAgain)
        {
            SceneManager.LoadScene(currentLevel);
            Cursor.visible = false;
        }
        else { SceneManager.LoadScene(currentLevel + 1); Cursor.visible = false; }
        
        //GUI elements are active on GameView
        scoreOnGameView.SetActive(true);
        dashText.SetActive(true);
        dashSlider.SetActive(true);
        timeOnGameView.SetActive(true);
        hourGlass.SetActive(true);
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
                Time.timeScale = 1;
                StartCoroutine(WaitToChangeScene(MenuStates.GAMEVIEW));
                break;
            case "Quit":

                //Turo added for skip howtoplay functionality
                SaveSystem.savingInstance.introIsSkipped = false;
                Time.timeScale = 1;
                StartCoroutine(WaitToChangeScene(MenuStates.STARTMENU));
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
                Time.timeScale = 1;
                MenuState = MenuStates.PAUSE;
                break;
        }
    }

    //Inactivates gameobjects from the UI
    public void ControlReturn()
    {
        if (pauseMenu.gameObject.activeSelf)
        {
            Cursor.visible = false;
            Time.timeScale = 1;
            pauseMenu.gameObject.SetActive(false);
        }
    }

    public void ControlGameOver(Transform transform)
    {
        gameOver.SetActive(true);
        //GUI elements not visible during GameOver
        scoreOnGameView.SetActive(false);
        dashText.SetActive(false);
        dashSlider.SetActive(false);
        timeOnGameView.SetActive(false);
        hourGlass.SetActive(false);

        if (win)
        {
            Cursor.visible = true;
            if (currentLevel != maxLevel - 1)   //Level finished is not the finalLevel
            {
                resultText.text = resultStringWin;
                gameOverText.text = gameOverStringWin;
                yesButton.SetActive(false);
                noButton.SetActive(false);
                quitButton.SetActive(true);
                quitFinalButton.SetActive(false);
                continueButton.SetActive(true);
            }
            else //FinalLevel
            {
                resultText.text = resultStringFinished;
                gameOverText.text = gameOverStringFinished;
                yesButton.SetActive(false);
                noButton.SetActive(false);
                quitButton.SetActive(false);
                quitFinalButton.SetActive(true);
                continueButton.SetActive(false);
                scoreText.SetActive(false);
            }
        }
        if (lose)   //Time has run out
        {
            Cursor.visible = true;
            resultText.text = resultStringLose;
            gameOverText.text = gameOverStringLose;
            yesButton.SetActive(true);
            noButton.SetActive(true);
            quitButton.SetActive(false);
            quitFinalButton.SetActive(false);
            continueButton.SetActive(false);
        }

        switch (transform.name)
        {
            case "Yes":
                tryAgain = true;
                MenuState = MenuStates.GAMEVIEW;
                break;
            case "No":
                StartCoroutine(WaitToChangeScene(MenuStates.HIGHSCORE));
                break;
            case "Start Over":
                tryAgain = true;
                MenuState = MenuStates.GAMEVIEW;
                break;
            case "Quit":
                //Turo added for skip howtoplay functionality
                SaveSystem.savingInstance.introIsSkipped = false;
                Time.timeScale = 1;
                StartCoroutine(WaitToChangeScene(MenuStates.STARTMENU));
                break;
            case "QuitFinal":
                StartCoroutine(WaitToChangeScene(MenuStates.HIGHSCORE));
                break;
            case "Continue":
                currentLevel = currentLevel + 1;
                StartCoroutine(WaitToChangeScene(MenuStates.GAMEVIEW));
                Debug.Log(currentLevel);
                break;
        }
    }

    public void ControlHighScore()
    {
        SceneManager.LoadScene("HighScoreScene");
    }

    public IEnumerator WaitToChangeScene(MenuStates state)  //Transition between scenes
    {
        Time.timeScale = 1;
        FindObjectOfType<SceneChange>().FadeIn();
        yield return new WaitForSeconds(3f);
        MenuState = state;
        Debug.Log(state);
    }

    private IEnumerator SceneChangeFade(MenuStates state)
    {
        Time.timeScale = 1;
        FindObjectOfType<SceneChange>().FadeIn();
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(currentLevel + 1);
    }
}
