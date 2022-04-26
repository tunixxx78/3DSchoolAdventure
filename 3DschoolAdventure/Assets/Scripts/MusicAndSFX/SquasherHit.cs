using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SquasherHit : MonoBehaviour
{
    public AudioSource squasherHit;

    private void Awake()
    {
        GetComponent<SquasherHit>();
    }

    public void HitDown()
    {
        squasherHit.Play();
    }
}
