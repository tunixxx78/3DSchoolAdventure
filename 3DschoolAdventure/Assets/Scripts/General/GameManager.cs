using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject resultPanel;
    private float startDelay;

    private void Start()
    {
        
        Time.timeScale = 1;

        // This checks if current scene is characterSelection scene and CharacterMenuController can be found.
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 1)
        {
            startDelay = FindObjectOfType<CharacterMenuController>().StartGameDelay;
        }
        
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartThisGame(int sceneNumber)
    {
        Cursor.visible = false;

        StartCoroutine(GoGoGo(sceneNumber));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator GoGoGo(int sceneNumber)
    {
        yield return new WaitForSeconds(startDelay);

        SceneManager.LoadScene(sceneNumber);
    }
}
