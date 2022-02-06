using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPack : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float forceAmount, jetPackTime, originalMoveSpeed;
    CharacterController myCC;
    public bool canFly = false;

    private void Awake()
    {
        myCC = player.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && canFly == false)
        {
            player.GetComponent<TuroPlayerMovement>().gravity = -1;
            originalMoveSpeed = player.GetComponent<TuroPlayerMovement>().moveSpeed;
            player.GetComponent<TuroPlayerMovement>().moveSpeed = forceAmount;
            StartCoroutine(GravityBack());
            canFly = true;
        }
        if(canFly == true)
        {
            
            //myCC.Move(Vector3.forward * forceAmount * Time.deltaTime);
        }
    }

    IEnumerator GravityBack()
    {
        yield return new WaitForSeconds(jetPackTime);

        player.GetComponent<TuroPlayerMovement>().gravity = -9.81f;
        player.GetComponent<TuroPlayerMovement>().moveSpeed = originalMoveSpeed;
        canFly = false;
    }
}
