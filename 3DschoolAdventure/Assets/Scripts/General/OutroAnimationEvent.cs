using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroAnimationEvent : MonoBehaviour
{
    MenuController menuController;
    [SerializeField] GameObject[] players;
 
    private void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        
    }

    public void EndOfYhisRun()
    {
        

        var activePlayer = PlayerPrefs.GetInt("MyCharacter");
        var player = players[activePlayer].GetComponent<TuroPlayerMovement>().currentPoints;


        PlayerPrefs.SetInt("PointsToNextLevel", player);

        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;

        menuController.win = true;

    }
}
