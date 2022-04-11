using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem savingInstance;

    public bool continueGame = false;

    private void Awake()
    {
        
        LoadData();

        if (savingInstance != null && savingInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        savingInstance = this;
        DontDestroyOnLoad(this);
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamedata.dat");
        GameData data = new GameData();

        data.continueGame = continueGame;

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


        }
    }
}

[Serializable]

class GameData
{
    public bool continueGame;
}