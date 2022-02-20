using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public TuroPlayerMovement playerScript;
    public int activePlayer;
    public GameObject[] players;

    public float dashNum;
    public GameObject[] dashImage;
    public Slider dashSlider;

    private void Awake()
    {
        activePlayer = PlayerPrefs.GetInt("MyCharacter");
        for (int i = 0; i < players.Length; i++)
        {
            playerScript = players[activePlayer].GetComponent<TuroPlayerMovement>();
            dashNum = playerScript.dashAmount;

            //Slider
            dashSlider.value = dashSlider.minValue;
        }
    }

    private void Update()
    {
        dashNum = playerScript.dashAmount;
        DashImageActivation();

        //Slider
        dashSlider.value = dashNum;
    }

    //Dashpanel's images
    private void DashImageActivation()
    {
        switch (dashNum)
        {
            case 0:
                dashImage[0].SetActive(false);
                dashImage[1].SetActive(false);
                dashImage[2].SetActive(false);
                break;
            case 1:
                dashImage[0].SetActive(true);
                dashImage[1].SetActive(false);
                dashImage[2].SetActive(false);
                break;
            case 2:
                dashImage[0].SetActive(true);
                dashImage[1].SetActive(true);
                dashImage[2].SetActive(false);
                break;
            case 3:
                dashImage[0].SetActive(true);
                dashImage[1].SetActive(true);
                dashImage[2].SetActive(true);
                break;
        }
    }
}
