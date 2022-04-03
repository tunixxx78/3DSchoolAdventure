using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCylinderSideCheck : MonoBehaviour
{
    public RotatingCylinder rotatingCylinderScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.tag == "CylinderLeft")
        {
            Debug.Log("vasen");
            rotatingCylinderScript.rotateLeft = true;
            rotatingCylinderScript.rotateRight = false;
        }
        if (other.gameObject.tag == "Player" && gameObject.tag == "CylinderRight")
        {
            Debug.Log("oikea");
            rotatingCylinderScript.rotateLeft = false;
            rotatingCylinderScript.rotateRight = true;
        }
    }
}
