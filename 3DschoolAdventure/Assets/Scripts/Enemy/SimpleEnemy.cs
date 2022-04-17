using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] NavMeshAgent simpleEnemy;
    [SerializeField] GameObject[] players;
    [SerializeField] float noticeDistance;

    Vector3 destination;

    private void Start()
    {
        // find active player in scene and set that to target
        var activePlayer = PlayerPrefs.GetInt("MyCharacter");
        player = players[activePlayer].GetComponent<TuroPlayerMovement>().transform;

        destination = simpleEnemy.destination;
    }

    private void Update()
    {
        destination = simpleEnemy.destination;


        if (Vector3.Distance (destination, player.position) < noticeDistance)
        {
            simpleEnemy.isStopped = false;
            simpleEnemy.SetDestination(player.position);
        }

        if (Vector3.Distance(destination, player.position) > noticeDistance)
        {
            simpleEnemy.isStopped = true;
        } 
        
    }
}
