using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField] Animator outAnimator, inAnimator;
    AudioSource music;
    [SerializeField] float fadeTime, currentVolume, startVolume, wantedVolume;

    private void Awake()
    {
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        currentVolume = 0;
        Time.timeScale = 1;
        
    }

    private void Start()
    {
        outAnimator.SetTrigger("Out");
        currentVolume = music.volume;
        StartCoroutine(FadeMusicOn());
    }

    public void FadeIn()
    {
        inAnimator.SetTrigger("In");

        StartCoroutine(FadeMusicOff());
    }

    public void FadeOut()
    {
        outAnimator.SetTrigger("Out");
        
    }

    private IEnumerator FadeMusicOff()
    {
        startVolume = music.volume;

        currentVolume = music.volume;


        while (currentVolume > 0)
        {
            Debug.Log("INESSÄ");
            currentVolume = currentVolume - 0.1f * Time.deltaTime;
            music.volume = currentVolume;

            yield return null;
        }

        music.Stop();
        music.volume = startVolume;
    }

    private IEnumerator FadeMusicOn()
    {
        startVolume = 0;

        currentVolume = 0;


        while (currentVolume < wantedVolume)
        {
            Debug.Log("INESSÄ");
            currentVolume = currentVolume + 0.1f * Time.deltaTime;
            music.volume = currentVolume;

            yield return null;
        }

        
    }

}
