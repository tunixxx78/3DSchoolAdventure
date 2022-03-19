using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingObstacle : MonoBehaviour
{
    //Pyörivät laatikot

    [SerializeField] Animator boxAnimator;
    bool turnLeft = false, turnRight = false, canRotate = false;
    


    private void Awake()
    {
        boxAnimator = GetComponentInParent<Animator>();

    }

    private void Start()
    {
        this.GetComponent<Transform>();
    }

    private void Update()
    {
        if (turnLeft)
        {
            boxAnimator.SetTrigger("TurnLeft");
        }
        if (turnRight)
        {
            boxAnimator.SetTrigger("TurnRight");
        }
        
    }

    private void OnTriggerEnter(Collider collider)

    {
        
        
    }


   
}
