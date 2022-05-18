using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] string fileName;
    public HighscoreHandler highscoreHandler;
    [SerializeField] string playerNameTwo;
    [SerializeField] TMP_Text playerPoints;
    int playerScore;
    [SerializeField] int testPoints;

    public List<InputEntry> entries = new List<InputEntry>();

    private void Awake()
    {
        if(SaveSystem.savingInstance.notFirstTimeToStart == false)
        {
            highscoreHandler.AddHighscoreIfPossible(new HighscoreElement("DevTeam", 215000));
            SaveSystem.savingInstance.notFirstTimeToStart = true;
            SaveSystem.savingInstance.SaveData();
        }

        testPoints = PlayerPrefs.GetInt("PointsToNextLevel");
        playerScore = testPoints;
        playerPoints.text = playerScore.ToString();
    }

    private void Update()
    {
        playerScore = testPoints;
        playerPoints.text = playerScore.ToString();
    }

    private void Start()
    {
        entries = FileHandler.ReadFromJson<InputEntry>(fileName);
        highscoreHandler = FindObjectOfType<HighscoreHandler>();
    }

    public void AddNameToList()
    {
        entries.Add(new InputEntry(nameInput.text, Random.Range(0, 100)));
        //nameInput.text = "";
        //FileHandler.SaveToJson<InputEntry>(entries, fileName);
        highscoreHandler.AddHighscoreIfPossible(new HighscoreElement(nameInput.text, playerScore));
    }
}
