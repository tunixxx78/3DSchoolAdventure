using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject[] dialoguePages;
    public int currentPage = 0;

    void Awake()
    {
        dialogue.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
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
            }
        }
    }

    public void NextButtonPressed()
    {
        currentPage++;
    }

}
