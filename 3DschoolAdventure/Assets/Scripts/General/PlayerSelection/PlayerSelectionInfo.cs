using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionInfo : MonoBehaviour
{
    public static PlayerSelectionInfo pSI;
    public int mySelectedCharacter;
    public GameObject[] allCharacters;

    private void OnEnable()
    {
        if(PlayerSelectionInfo.pSI == null)
        {
            PlayerSelectionInfo.pSI = this;
        }
        else
        {
            if(PlayerSelectionInfo.pSI != this)
            {
                Destroy(PlayerSelectionInfo.pSI.gameObject);
                PlayerSelectionInfo.pSI = this;
            }

        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("MyCharacter"))
        {
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");
        }
        else
        {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
        }
    }
}
