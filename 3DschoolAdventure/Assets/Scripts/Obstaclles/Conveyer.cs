using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    [SerializeField] float conveyerSpeed;
    GameObject playerInConveyer;
    CharacterController collisionCC;
    bool playerIsInBelt = false;
    [SerializeField] bool movingForward = false, movingBackWard = false;

    private void Update()
    {
        if (playerIsInBelt && movingForward)
        {
            collisionCC.Move(transform.forward * conveyerSpeed * Time.deltaTime);
        }
        if (playerIsInBelt && movingBackWard)
        {
            collisionCC.Move(-transform.forward * conveyerSpeed * Time.deltaTime);
        }
        

    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerInConveyer = collider.gameObject;
            collisionCC = collider.GetComponent<TuroPlayerMovement>().myCC;

            collisionCC.Move(transform.forward * conveyerSpeed * Time.deltaTime);
            playerIsInBelt = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerIsInBelt = false;
        }
    }
}
