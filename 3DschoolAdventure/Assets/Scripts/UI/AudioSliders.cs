using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliders : MonoBehaviour
{
    public AudioMixer musicAudioMixer, effectsAudioMixer;
    public Slider musicSlider, effectsSlider;

    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVol", 0.75f);
    }

    public void SetMusicLevel(float musicSlider)
    {
        musicAudioMixer.SetFloat("MusicVol", Mathf.Log10(musicSlider) * 20);
        PlayerPrefs.SetFloat("MusicVol", musicSlider);
    }

    public void SetEffectsLevel(float effectsSlider)
    {
        effectsAudioMixer.SetFloat("EffectsVol", Mathf.Log10(effectsSlider) * 20);
        PlayerPrefs.SetFloat("EffectsVol", effectsSlider);
    }

    public void MuteButtonPressed()
    {
        musicSlider.value = 0;
        effectsSlider.value = 0;
    }
}
