using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliders : MonoBehaviour
{
    public AudioMixer musicAudioMixer, effectsAudioMixer;
    public Slider musicSlider, effectsSlider;
    public bool muted;
    public float musicV, sfxV;

    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", musicV);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVol", musicV);
    }

    public void SetMusicLevel(float musicSlider)
    {
        musicAudioMixer.SetFloat("MusicVol", Mathf.Log10(musicSlider) * 20);
        PlayerPrefs.SetFloat("MusicVol", musicSlider);
        musicV = PlayerPrefs.GetFloat("MusicVol", musicSlider);
    }

    public void SetEffectsLevel(float effectsSlider)
    {
        effectsAudioMixer.SetFloat("EffectsVol", Mathf.Log10(effectsSlider) * 20);
        PlayerPrefs.SetFloat("EffectsVol", effectsSlider);
        sfxV = PlayerPrefs.GetFloat("EffectsVol", effectsSlider);
    }

    public void MuteButtonPressed()
    {
        if (muted)
        {
            musicSlider.value = 1;
            effectsSlider.value = 1;
            muted = false;
        }
        else
        {
            musicSlider.value = 0.0001f;
            effectsSlider.value = 0.0001f;
            muted = true;
        }
    }
}
