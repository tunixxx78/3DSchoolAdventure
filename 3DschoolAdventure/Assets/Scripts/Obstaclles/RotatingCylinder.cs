using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCylinder : MonoBehaviour
{
    public bool rotating;
    public float rotatingSpeed = 1f;
    //public CharacterController player;
    public Vector3 playerPos;
    public bool rotateRight, rotateLeft;

    void Awake()
    {
        //GetComponent<TuroPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotating && rotateLeft)
        {
            transform.Rotate(/*-1 * player.GetComponent<TuroPlayerMovement>().gravity * player.GetComponent<TuroPlayerMovement>().jumpForce * */rotatingSpeed * Time.deltaTime, 0f, 0f, Space.World);
        }
        if (rotating && rotateRight)
        {
            transform.Rotate(-1 /* * player.GetComponent<TuroPlayerMovement>().gravity * player.GetComponent<TuroPlayerMovement>().jumpForce*/ * rotatingSpeed * Time.deltaTime, 0f, 0f, Space.World);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerPos = other.gameObject.transform.position;
            //player = other.GetComponent<TuroPlayerMovement>().myCC;
            rotating = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        rotating = false;
    }
}
