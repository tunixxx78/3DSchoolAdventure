using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem savingInstance;

    public bool continueGame = false, introIsSkipped = false, notFirstTimeToPlay = false;

    private void Awake()
    {
        

        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 0)
        {
            PlayerPrefs.SetInt("PointsToNextLevel", 0);
        }

            LoadData();

        if (savingInstance != null && savingInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        savingInstance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            notFirstTimeToPlay = false;
            SaveData();
        }
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamedata.dat");
        GameData data = new GameData();

        data.continueGame = continueGame;
        data.notFirstTimeToPlay = notFirstTimeToPlay;

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/gamedata.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamedata.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);

            continueGame = data.continueGame;
            notFirstTimeToPlay = data.notFirstTimeToPlay;


        }
    }
}

[Serializable]

class GameData
{
    public bool continueGame, introIsSkipped, notFirstTimeToPlay;
}