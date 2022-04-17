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
    //private Transform originalRot;

    public TuroPlayerMovement playerScript;
    public int activePlayer;
    public GameObject[] players;

    void Awake()
    {
        //originalRot = this.gameObject.transform.localRotation;
        activePlayer = PlayerPrefs.GetInt("MyCharacter");
        for (int i = 0; i < players.Length; i++)
        {
            playerScript = players[activePlayer].GetComponent<TuroPlayerMovement>();
        }
    }

    void Update()
    {
        if (canRotate)
        {
            if (rotateLeft)
            {
                transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime);
            }
            if (rotateRight)
            {
                transform.Rotate(Vector3.down, rotatingSpeed * Time.deltaTime);
            }
        }
        //if (transform.eulerAngles.x >= 60)
        //{
        //    Debug.Log("CC off");
        //    playerScript.myCC.enabled = true;
        //}
        //if (transform.eulerAngles.x <= -60)
        //{
        //    Debug.Log("CC off");
        //    playerScript.myCC.enabled = true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canRotate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canRotate = false;
            //StartCoroutine(ResetTransform());
        }
    }

    //IEnumerator ResetTransform()
    //{
    //    yield return new WaitForSeconds(3);

    //    transform.rotation = Quaternion.Euler(-90, 180, 0);
    //    //transform.rotation = Quaternion.identity;
    //}

}
