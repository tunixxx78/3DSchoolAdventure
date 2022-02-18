using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smasher : MonoBehaviour
{
    [SerializeField] Rigidbody objectRb;
    [SerializeField] bool hasHit = false;
    [SerializeField] float force, timeToMove;

    private void Awake()
    {
        objectRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (hasHit)
        {
            objectRb.AddForce(transform.up * force * Time.deltaTime);
            //Debug.Log("Sitten nostetaan! ");

            StartCoroutine(TimeToMoveUp(timeToMove));
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Walkable")
        {
            Debug.Log("Osuma");

            hasHit = true;

        }
    }

    IEnumerator TimeToMoveUp(float movingTime)
    {
        yield return new WaitForSeconds(movingTime);

        hasHit = false;

    }
    
}
