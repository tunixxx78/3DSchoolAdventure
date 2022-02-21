using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSFXController : MonoBehaviour
{
    [SerializeField] AudioSource rightStep, leftStep;

    public void PlayerRightStep()
    {
        rightStep.Play();
    }

    public void PlayerLeftStep()
    {
        leftStep.Play();
    }
}
