using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCylinder : MonoBehaviour
{
    public bool canRotate = false;
    public float rotatingSpeed = 1f;
    //public GameObject player;
    //public Vector3 playerPos;
    public bool rotateRight, rotateLeft;

    public TuroPlayerMovement playerScript;
    public int activePlayer;
    public GameObject[] players;

    void Awake()
    {
        activePlayer = PlayerPrefs.GetInt("MyCharacter");
        for (int i = 0; i < players.Length; i++)
        {
            playerScript = players[activePlayer].GetComponent<TuroPlayerMovement>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate && rotateLeft)
        {
            transform.Rotate(Vector3.back, /*-1 * player.GetComponent<TuroPlayerMovement>().gravity * player.GetComponent<TuroPlayerMovement>().jumpForce * */rotatingSpeed * Time.deltaTime);
        }
        if (canRotate && rotateRight)
        {
            transform.Rotate(Vector3.forward, /* * player.GetComponent<TuroPlayerMovement>().gravity * player.GetComponent<TuroPlayerMovement>().jumpForce*/rotatingSpeed * Time.deltaTime);
        }
        if (transform.eulerAngles.z >= 60)
        {
            Debug.Log("CC off");
            playerScript.myCC.enabled = true;
        }
        if (transform.eulerAngles.z <= -60)
        {
            Debug.Log("CC off");
            playerScript.myCC.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //playerPos = other.gameObject.transform.position;
            //player = other.GetComponent<TuroPlayerMovement>().myCC;
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

        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

}
