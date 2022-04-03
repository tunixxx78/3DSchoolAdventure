using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform teleportSpawnPoint;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("TELEPORTISSA");
            Debug.Log(collider);
            collider.transform.position = new Vector3(teleportSpawnPoint.position.x, teleportSpawnPoint.position.y, teleportSpawnPoint.position.z);
        }
    }
}
