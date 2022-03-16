using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [SerializeField] float rotatingSpeed, rotatingBreak;
    bool canRotate;
    

    private void Start()
    {
        canRotate = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.Rotate(Vector3.down * rotatingSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.P))
        {
            canRotate = false;
        }
        if (Input.GetKey(KeyCode.O))
        {
            canRotate = true;
        }

    }
    /*
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            canRotate = false;

            StartCoroutine(StartRotating());
        }
    }
    */

    IEnumerator StartRotating()
    {
        yield return new WaitForSeconds(rotatingBreak);

        canRotate = true;
    }
}
