using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourglassController : MonoBehaviour
{
    public Slider hourGlass;
    public float currentTime;
    public TuroPlayerMovement playerMovement;

    private void Awake()
    {
        hourGlass.value = playerMovement.startTime;
        currentTime = hourGlass.value;
        hourGlass.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void Update()
    {
        currentTime = playerMovement.currentTime;
        hourGlass.value = currentTime;
    }

    private void ValueChangeCheck()
    {
        Debug.Log(hourGlass.value);
    }

    private void AddTime()
    {
        
    }

    private void LoseTime()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Collectible")
        {
            hourGlass.value += collider.GetComponent<Collectibles>().timeAmount;
        }
    }
}
