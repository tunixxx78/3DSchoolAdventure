using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUI : MonoBehaviour
{
    public TuroPlayerMovement playerScript;
    public int activePlayer;
    public GameObject[] players;

    public int dashNum;
    public GameObject[] dashImage;

    private void Awake()
    {
        activePlayer = PlayerPrefs.GetInt("MyCharacter");
        for (int i = 0; i < players.Length; i++)
        {
            playerScript = players[activePlayer].GetComponent<TuroPlayerMovement>();
            dashNum = playerScript.dashAmount;
        }
    }

    private void Update()
    {
        dashNum = playerScript.dashAmount;
        DashImageActivation();
    }

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
