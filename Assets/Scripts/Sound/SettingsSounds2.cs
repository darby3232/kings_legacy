using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsSounds2 : MonoBehaviour
{
    public AudioMixer audioMixer;

    public bool soundOn = true;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume3", volume);
    }

    public void ToggleAudio(float returnVolume)
    {
        if (soundOn)
        {
            audioMixer.SetFloat("Volume3", -80);
            soundOn = false;
        }
        else
        {
            audioMixer.SetFloat("Volume3", GameObject.Find("Sound Slider").GetComponent<Slider>().value);
            soundOn = true;
        }
    }

}
