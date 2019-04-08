using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsSounds : MonoBehaviour
{
    public AudioMixer audioMixer;

    public bool soundOn = true;

    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume", volume);
    }

    public void ToggleAudio(float returnVolume)
    {
        if (soundOn)
        {
            audioMixer.SetFloat("Volume", -80);
            soundOn = false;
        }
        else
        {
            audioMixer.SetFloat("Volume", GameObject.Find("Volume Slider").GetComponent<Slider>().value);
            soundOn = true;
        }
    }

}
