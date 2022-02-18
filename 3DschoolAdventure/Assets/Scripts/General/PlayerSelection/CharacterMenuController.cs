using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenuController : MonoBehaviour
{
    public void OnClickedCharacterPick(int whichCharacter)
    {
        if(PlayerSelectionInfo.pSI != null)
        {
            PlayerSelectionInfo.pSI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }
}
