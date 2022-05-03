using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamRun : MonoBehaviour
{
    [SerializeField] Animator introcam;
    [SerializeField] GameObject points, dash, time, timeIcon, introCam;
    [SerializeField] GameObject[] players;

    CharacterController playerCC;
    TuroPlayerMovement playerMovement;


    private void Awake()
    {
        var activePlayer = PlayerPrefs.GetInt("MyCharacter");
        playerMovement = players[activePlayer].GetComponent<TuroPlayerMovement>();
        playerCC = players[activePlayer].GetComponent<CharacterController>();
    }

    private void Start()
    {
        if(SaveSystem.savingInstance.introIsSkipped == false)
        {
            playerMovement.enabled = false;
            playerCC.enabled = false;
        }
        if (SaveSystem.savingInstance.introIsSkipped == true)
        {
            points.SetActive(true);
            dash.SetActive(true);
            time.SetActive(true);
            timeIcon.SetActive(true);

            playerMovement.enabled = true;
            playerCC.enabled = true;

            introCam.SetActive(false);
        }
        
    }

    public void StartCamRunAnimation()
    {
        introcam.SetTrigger("Start");

        points.SetActive(true);
        dash.SetActive(true);
        time.SetActive(true);
        timeIcon.SetActive(true);
    }

    public void StopCamRunAnimation()
    {
        playerMovement.enabled = true;
        playerCC.enabled = true;

        introCam.SetActive(false);
        
    }
}
