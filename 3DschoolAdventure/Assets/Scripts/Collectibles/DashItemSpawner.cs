using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject dashItem;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] List<GameObject> dashItems = new List<GameObject>();


    public void SpawnDashItems()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject dashInstance =  Instantiate(dashItem, spawnPoint.position, Quaternion.identity);
            dashItems.Add(dashInstance);
        }
    }

    public void ClearDashItemList()
    {
        foreach (GameObject item in dashItems)
        {
            Destroy(item);
        }
        dashItems.Clear();
    }
}
