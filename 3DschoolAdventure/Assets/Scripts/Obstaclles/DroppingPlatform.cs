using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingPlatform : MonoBehaviour
{
    [SerializeField] Transform endPoint;
    [SerializeField] float moveSpeed, timeToHideSighn = 1;
    [SerializeField] GameObject sighn;
    bool sighnIsShowned = false;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(sighnIsShowned == false)
            {
                sighnIsShowned = true;
                transform.position = new Vector3(endPoint.position.x, endPoint.position.y, endPoint.position.z);
                sighn.SetActive(true);
                StartCoroutine(HideSighns());
            }
            else
            {

            }
            
        }
    }

    IEnumerator HideSighns()
    {
        yield return new WaitForSeconds(timeToHideSighn);

        sighn.SetActive(false);
    }

}
