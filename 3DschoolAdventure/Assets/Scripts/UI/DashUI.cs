using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUI : MonoBehaviour
{
    public int dashNum;
    public TuroPlayerMovement playerScript;
    public int activePlayer;
    public GameObject[] players;

    private void Awake()
    {
        activePlayer = PlayerPrefs.GetInt("MyCharacter");
        for (int i = 0; i < players.Length; i++)
        {
            playerScript = players[activePlayer].GetComponent<TuroPlayerMovement>();
            dashNum = playerScript.dashAmount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        dashNum = playerScript.dashAmount;
    }
}
