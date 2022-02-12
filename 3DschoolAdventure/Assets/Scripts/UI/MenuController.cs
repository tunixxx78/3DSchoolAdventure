using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
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

    public void ChangeState()
    {
        switch (menuState)
        {
            case MenuStates.STARTMENU:
                ControlMainMenu();
                break;
            case MenuStates.GAMEVIEW:
                ControlGameView();
                break;
            case MenuStates.PAUSE:
                ControlPauseMenu();
                break;
            case MenuStates.SETTINGS:
                ControlSettingsMenu();
                break;
        }
    }

    public void ControlMainMenu()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("StartMenu"))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    public void ControlGameView()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("TuronTestScene"))
        {
            SceneManager.LoadScene("TuronTestScene");
        }
    }

    public void ControlPauseMenu()
    {

    }

    public void ControlSettingsMenu()
    {

    }
}
