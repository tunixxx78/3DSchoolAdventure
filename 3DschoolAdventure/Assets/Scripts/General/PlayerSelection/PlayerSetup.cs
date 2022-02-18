using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] GameObject pAOne, pATwo, pAThree, camOne, camTwo, camThree;
    [SerializeField] int sellectedPlayerNumber;

    private void Awake()
    {
        sellectedPlayerNumber = PlayerPrefs.GetInt("MyCharacter");
    }

    private void Start()
    {
        if(sellectedPlayerNumber == 0)
        {
            pAOne.SetActive(true);
            camOne.SetActive(true);
            pATwo.SetActive(false);
            pAThree.SetActive(false);
            camTwo.SetActive(false);
            camThree.SetActive(false);
        }
        if(sellectedPlayerNumber == 1)
        {
            pAOne.SetActive(false);
            camOne.SetActive(false);
            pATwo.SetActive(true);
            pAThree.SetActive(false);
            camTwo.SetActive(true);
            camThree.SetActive(false);
        }
        if(sellectedPlayerNumber == 2)
        {
            pAOne.SetActive(false);
            camOne.SetActive(false);
            pATwo.SetActive(false);
            pAThree.SetActive(true);
            camTwo.SetActive(false);
            camThree.SetActive(true);
        }
    }
}
