using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourglassController : MonoBehaviour
{
    public Slider hourGlass;
    public float currentTime;
    public TuroPlayerMovement playerScript;
    public int activePlayer;
    public GameObject[] players;

    private void Awake()
    {
        activePlayer = PlayerPrefs.GetInt("MyCharacter");
        for (int i = 0; i < players.Length; i++)
        {
            playerScript = players[activePlayer].GetComponent<TuroPlayerMovement>();
            hourGlass.value = playerScript.startTime;
            currentTime = hourGlass.value;
            //hourGlass.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }
    }

    private void Update()
    {
        currentTime = playerScript.currentTime;
        hourGlass.value = currentTime;
    }

    //private void ValueChangeCheck()
    //{
    //    Debug.Log(hourGlass.value);
    //}
}
