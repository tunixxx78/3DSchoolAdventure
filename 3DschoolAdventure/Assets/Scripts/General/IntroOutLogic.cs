using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroOutLogic : MonoBehaviour
{
    [SerializeField] GameObject SceneChangeOut, sceneChangeIn;

    private void Awake()
    {
        if (SaveSystem.savingInstance.introIsSkipped == true)
        {
            sceneChangeIn.SetActive(true);
        }
    }

    private void Start()
    {
        if(SaveSystem.savingInstance.introIsSkipped == true)
        {
            //sceneChangeIn.SetActive(true);
        }
    }

    public void ShowOutAnimation()
    {
        SceneChangeOut.SetActive(true);
    }


}
