using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSurface : MonoBehaviour
{
    [SerializeField] float slidingSpeed;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.GetComponent<TuroPlayerMovement>().myCC.Move(transform.forward * slidingSpeed * Time.deltaTime);
        }
    }
}
