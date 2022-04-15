using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRotator : MonoBehaviour
{
    [SerializeField] float speed;
    bool canRotate = false;
    [SerializeField] GameObject player;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.Rotate(Vector3.down, speed * Time.deltaTime);
        }
        if (transform.eulerAngles.x >= 60)
        {
            Debug.Log("TÄÄLLÄOLLAANJEEEEE");
            player.GetComponent<TuroPlayerMovement>().myCC.enabled = true;
            //player.GetComponent<TuroPlayerMovement>().ResetPlayerRotations();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            canRotate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canRotate = false;
            StartCoroutine(ResetTransform());
        }
    }

    IEnumerator ResetTransform()
    {
        yield return new WaitForSeconds(2);

        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
}
