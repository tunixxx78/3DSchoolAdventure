using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingObstacle : MonoBehaviour
{
    [SerializeField] float rollingSpeed, rollingTime;
    bool canRotate = false;

    private void Start()
    {
        this.GetComponent<Transform>();
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.Rotate(Vector3.right, rollingSpeed);
        }
        
    }

    private void OnTriggerEnter(Collider collider)

    {
        if(collider.gameObject.tag == "Player")
        {
            canRotate = true;
            StartCoroutine(StopRolling());
        }
    }


    IEnumerator StopRolling()
    {
        yield return new WaitForSeconds(rollingTime);

            canRotate = false;
        
        
    }
}
