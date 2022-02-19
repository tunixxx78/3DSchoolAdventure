using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingPlatform : MonoBehaviour
{
    Animator platformAnimator;
    bool spaceIsPressed = false;
    [SerializeField] float wait;

    private void Awake()
    {
        platformAnimator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && spaceIsPressed == false)
        {
            platformAnimator.SetTrigger("Flip");
            spaceIsPressed = true;
            StartCoroutine(ReturnNotPressed(wait));
        }
    }

    IEnumerator ReturnNotPressed(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        spaceIsPressed = false;
    }
    
}
