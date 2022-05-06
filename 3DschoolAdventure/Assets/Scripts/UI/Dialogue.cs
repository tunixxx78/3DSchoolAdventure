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
            else
            {
                
                dialogue.SetActive(false);
                Time.timeScale = 1;
                Cursor.visible = false;

                intro.StartCamRunAnimation();
            }
        }
    }

    public void NextButtonPressed()
    {
        currentPage++;
    }

}
