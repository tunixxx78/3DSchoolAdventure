using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField] Animator characterSelectionAnimator;
    public float StartGameDelay;

    private void Start()
    {
        PlayerPrefs.SetInt("MyCharacter", 1);
        characterSelectionAnimator.SetBool("C1Active", true);
    }

    public void OnClickedCharacterPick(int whichCharacter)
    {
        if(PlayerSelectionInfo.pSI != null)
        {
            PlayerSelectionInfo.pSI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
            Debug.Log(whichCharacter + "Is sellected!");


            FindObjectOfType<PlayerSelectionInfo>().ReadyToStartGame();
        }
    }

    public void SelectPlayerOne()
    {
        string player = "C0Active";
        characterSelectionAnimator.SetTrigger("TriggerC0");

        StartCoroutine(ActivateNewPlayer(player));
    }

    public void SelectPlayerTwo()
    {
        string player = "C1Active";
        characterSelectionAnimator.SetTrigger("TriggerC1");

        StartCoroutine(ActivateNewPlayer(player));
    }

    public void SelectPlayerThree()
    {
        string player = "C2Active";
        characterSelectionAnimator.SetTrigger("TriggerC2");

        StartCoroutine(ActivateNewPlayer(player));
    }

    public void GameOn()
    {
        characterSelectionAnimator.SetTrigger("Play");
    }

    private void DeActivateAllPlayers()
    {
        characterSelectionAnimator.SetBool("C0Active", false);
        characterSelectionAnimator.SetBool("C1Active", false);
        characterSelectionAnimator.SetBool("C2Active", false);
    }

    IEnumerator ActivateNewPlayer(string player)
    {
        yield return new WaitForSeconds(1);

        DeActivateAllPlayers();

        characterSelectionAnimator.SetBool(player, true);
    }
}
