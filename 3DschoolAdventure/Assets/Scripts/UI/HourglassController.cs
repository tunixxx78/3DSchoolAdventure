using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HourglassController : MonoBehaviour
{
    public Slider hourGlass;
    public float currentTime;
    public TuroPlayerMovement playerScript;
    public int activePlayer;
    public GameObject[] players;

    public TMP_Text timeText;

    private void Awake()
    {
        activePlayer = PlayerPrefs.GetInt("MyCharacter");
        for (int i = 0; i < players.Length; i++)
        {
            playerScript = players[activePlayer].GetComponent<TuroPlayerMovement>();
            hourGlass.value = playerScript.startTime;
            currentTime = hourGlass.value;
            //timeText.text = playerScript.startTime.ToString(string.Format("{0:0}", playerScript.startTime));
        }
    }

    private void Update()
    {
        currentTime = playerScript.currentTime;

        float min = Mathf.FloorToInt(currentTime / 60);
        float sec = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", min, sec);

        hourGlass.value = currentTime;
    }

    //private void TimeText()
    //{
    //    if (currentTime <= 9 && currentTime > 0)
    //    {
    //        timeText.gameObject.SetActive(true);
    //        timeText.text = currentTime.ToString(string.Format("{0:0}", currentTime));
    //    }
    //    else
    //    {
    //        timeText.gameObject.SetActive(false);
    //        timeText.text = 9.ToString();
    //    }
    //}
}
