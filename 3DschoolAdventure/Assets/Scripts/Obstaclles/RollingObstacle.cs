using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingObstacle : MonoBehaviour
{
    //Pyörivät laatikot

    [SerializeField] Animator boxAnimator;
    public bool turnLeft = false, turnRight = false, canRotate = false, animationIsPlaying = false;
    


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
        /*
        if (turnLeft && canRotate)
        {
            boxAnimator.SetTrigger("TurnLeft");
        }
        if (turnRight && canRotate)
        {
            boxAnimator.SetTrigger("TurnRight");
        }
        */
    }

    private void OnTriggerEnter(Collider collider)

    {
        
        if (collider.gameObject.tag == "Player" && turnLeft && animationIsPlaying == false)
        {
            canRotate = false;
            animationIsPlaying = true;
            boxAnimator.SetTrigger("TurnLeft");
            StartCoroutine(AnimationIsPlayed());
        }
        if (collider.gameObject.tag == "Player" && turnRight && animationIsPlaying == false)
        {
            canRotate = false;
            animationIsPlaying = true;
            boxAnimator.SetTrigger("TurnRight");
            StartCoroutine(AnimationIsPlayed());
        }
    }

    IEnumerator AnimationIsPlayed()
    {
        yield return new WaitUntil(() => canRotate == true);
        //yield return new WaitForSeconds(4);

        GetComponentInParent<RollingBoxAnimationEvents>().TurnAnimationIsPlayingToFalseAgain();
        
    }

    public void CanRotateToTrue()
    {
        Debug.Log("PERILLÄ");
        canRotate = true;
    }


   
}
