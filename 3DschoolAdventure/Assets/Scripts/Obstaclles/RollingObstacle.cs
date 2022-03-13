using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingObstacle : MonoBehaviour
{
    //Pyörivät laatikot

    [SerializeField] float rollingSpeed, rollingTime;
    bool canRotate = false, isRotating = false;
    [SerializeField] Rigidbody rb;


    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Start()
    {
        this.GetComponent<Transform>();
    }

    private void Update()
    {

        if (canRotate)
        {
            rb.transform.Rotate(Vector3.right, rollingSpeed);
        }
        
    }

    private void OnTriggerEnter(Collider collider)

    {
        if(collider.gameObject.tag == "Player" && isRotating == false)
        {
            canRotate = true;
            StartCoroutine(StopRolling());
        }
        else { return; }
    }


    IEnumerator StopRolling()
    {
        yield return new WaitForSeconds(rollingTime);

            canRotate = false;
        
        
    }
}
