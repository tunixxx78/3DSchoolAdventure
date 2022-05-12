using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject[] dialoguePages;
    public int currentPage = 0;

    //for turon intronCamRun
    IntroCamRun intro;
    

    void Awake()
    {
        if(SaveSystem.savingInstance.notFirstTimeToPlay == false)
        {
            dialogue.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;

            //for turon introcamrun

            intro = FindObjectOfType<IntroCamRun>();
        }

        else if(SaveSystem.savingInstance.introIsSkipped == false && SaveSystem.savingInstance.notFirstTimeToPlay == true)
        {
            dialogue.SetActive(true);
            FindObjectOfType<MenuController>().dialogueActive = true;
            Cursor.visible = true;
            Time.timeScale = 0;

            //for turon introcamrun

            intro = FindObjectOfType<IntroCamRun>();
        }
        
    }

    private void Update()
    {
        for (int i = 0; i < dialoguePages.Length; i++)
        {
            if (currentPage < dialoguePages.Length)
            {
                dialoguePages[currentPage].SetActive(true);
                if (currentPage > 0)
                {
                    dialoguePages[currentPage - 1].SetActive(false);
                }
            }
            else if (currentPage == dialoguePages.Length)
            {   
                dialogue.SetActive(false);

                intro.StartCamRunAnimation();
                FindObjectOfType<MenuController>().dialogueActive = false;
                //Time.timeScale = 1;
                if (!dialogue.activeSelf && (FindObjectOfType<MenuController>().MenuState == MenuStates.GAMEOVER || FindObjectOfType<MenuController>().MenuState == MenuStates.PAUSE || FindObjectOfType<MenuController>().MenuState == MenuStates.SETTINGS))
                {
                    Cursor.visible = true;
                }
                else if (!dialogue.activeSelf && (FindObjectOfType<MenuController>().MenuState != MenuStates.GAMEOVER || FindObjectOfType<MenuController>().MenuState != MenuStates.PAUSE || FindObjectOfType<MenuController>().MenuState != MenuStates.SETTINGS))
                {
                    Cursor.visible = false;
                }
            }

        }

    }

    public void NextButtonPressed()
    {
        currentPage++;
    }

}
