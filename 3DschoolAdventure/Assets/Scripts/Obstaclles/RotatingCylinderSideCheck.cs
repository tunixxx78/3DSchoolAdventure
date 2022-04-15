using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCylinderSideCheck : MonoBehaviour
{
    public RotatingCylinder rotatingCylinderScript;
    private float moveSpeed = 3f;
    private bool hasHitCylinder = false;
    public CharacterController playerCC;

    //private void Start()
    //{
    //    playerCC = rotatingCylinderScript.playerScript.myCC;
    //}

    private void Update()
    {
        if (hasHitCylinder)
        {
            if (rotatingCylinderScript.rotateLeft && !rotatingCylinderScript.rotateRight)
            {
                playerCC.Move(transform.forward * -moveSpeed * Time.deltaTime);
            }

            if (rotatingCylinderScript.rotateRight && !rotatingCylinderScript.rotateLeft)
            {
                playerCC.Move(transform.forward * moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerCC = other.GetComponent<TuroPlayerMovement>().myCC;
        hasHitCylinder = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.tag == "CylinderLeft")
        {
            //playerCC.Move(transform.forward * -moveSpeed * Time.deltaTime);
            Debug.Log("vasen");
            rotatingCylinderScript.rotateLeft = true;
            rotatingCylinderScript.rotateRight = false;
        }
        if (other.gameObject.tag == "Player" && gameObject.tag == "CylinderRight")
        {
            //playerCC.Move(transform.forward * moveSpeed * Time.deltaTime);
            Debug.Log("oikea");
            rotatingCylinderScript.rotateLeft = false;
            rotatingCylinderScript.rotateRight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        rotatingCylinderScript.rotateLeft = false;
        rotatingCylinderScript.rotateRight = false;
        hasHitCylinder = false;
    }
}
