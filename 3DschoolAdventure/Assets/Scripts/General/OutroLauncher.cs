using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroLauncher : MonoBehaviour
{
    [SerializeField] GameObject outroCam, enemys, players;
    PathOpener pathOpener;
    [SerializeField] GameObject[] playeer;

    private void Awake()
    {
        pathOpener = FindObjectOfType<PathOpener>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var activePlayer = PlayerPrefs.GetInt("MyCharacter");
            var player = playeer[activePlayer].GetComponent<TuroPlayerMovement>();

            player.playerAnimator.SetBool("Run", false);

            Debug.Log(collider + "OSUI collideriin!");
            collider.GetComponent<TuroPlayerMovement>().enabled = false;

            pathOpener.timer = 0;
            //players.SetActive(false);
            outroCam.SetActive(true);
            enemys.SetActive(false);
            

        }
    }
}
