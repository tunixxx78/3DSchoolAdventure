using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreHandler : MonoBehaviour
{
    List<HighscoreElement> highscoreList = new List<HighscoreElement>();
    [SerializeField] int maxCount = 5;
    [SerializeField] string fileName;

    public delegate void OnHighscoreListChanged(List<HighscoreElement> list);
    public static event OnHighscoreListChanged onHighscoreListChanged;

    private void Start()
    {
        LoadHighscore();
    }

    private void LoadHighscore()
    {
        highscoreList = FileHandler.ReadFromJson<HighscoreElement>(fileName);

        while (highscoreList.Count > maxCount)
        {
            highscoreList.RemoveAt(maxCount);
        }
        if (onHighscoreListChanged != null)
        {
            onHighscoreListChanged.Invoke(highscoreList);
        }
    }

    private void SaveHighscore()
    {
        FileHandler.SaveToJson<HighscoreElement>(highscoreList, fileName);

    }

    public void AddHighscoreIfPossible(HighscoreElement element)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if(i >= highscoreList.Count || element.points > highscoreList[i].points)
            {
                highscoreList.Insert(i, element);

                while (highscoreList.Count > maxCount)
                {
                    highscoreList.RemoveAt(maxCount);
                }

                SaveHighscore();

                if (onHighscoreListChanged != null)
                {
                    onHighscoreListChanged.Invoke(highscoreList);
                }

                break;
            }
        }
    }

    //exiting back to main menu(temporary function)

    public void BackToMainMenu()
    {
        PlayerPrefs.SetInt("PointsToNextLevel", 0);
        SceneManager.LoadScene(0);
        SaveSystem.savingInstance.continueGame = false;
        SaveSystem.savingInstance.SaveData();
    }
}
