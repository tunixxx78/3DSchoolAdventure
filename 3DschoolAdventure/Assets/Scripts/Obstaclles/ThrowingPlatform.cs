using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingPlatform : MonoBehaviour
{
    public float BounceForce;
    public ParticleSystem jumpper;
   

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            jumpper.Play(jumpper);
        }
        
    }
    

}
