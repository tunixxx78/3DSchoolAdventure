using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathOpener : MonoBehaviour
{
    [SerializeField] GameObject path, pathTimerObject;
    public float timer, timeToShowPath;
    [SerializeField] TMP_Text pathTimer;

    bool pathIsOn = false;

    SoundFX soundFX;

    private void Awake()
    {
        soundFX = FindObjectOfType<SoundFX>();
    }

    private void Start()
    {
        timer = timeToShowPath;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Destroy(this.gameObject, timeToShowPath);
            pathIsOn = true;
            
        }

        if (pathIsOn)
        {
            path.SetActive(true);
            string tempTimer = string.Format("{0:00}", timer);
            pathTimer.text = tempTimer;
            pathTimerObject.SetActive(true);

            timer -= 1 * Time.deltaTime;
        }

        if(timer <= 0)
        {
            soundFX.secretPathClose.Play();
            pathIsOn = false;
            timer = timeToShowPath;
            pathTimerObject.SetActive(false);
            path.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GetComponent<CapsuleCollider>().enabled = false;
            soundFX.secretPathOpen.Play();
            Destroy(this.gameObject, timeToShowPath);
            pathIsOn = true;
        }
    }
}
