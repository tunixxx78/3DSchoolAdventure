using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class IntroSFX : MonoBehaviour
{
    [SerializeField] AudioSource voice1, voice2, voice3;
    [SerializeField] AudioSource glass, wush;

    public void TalkingOne()
    {
        voice1.Play();
    }

    public void TalkingTwo()
    {
        voice2.Play();
    }

    public void TalkingThree()
    {
        voice3.Play();
    }

    public void Glass()
    {
        glass.Play();
    }

    public void Rotating()
    {
        wush.Play();
    }
}
