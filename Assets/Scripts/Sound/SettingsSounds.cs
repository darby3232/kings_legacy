﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsSounds : MonoBehaviour
{
    public AudioMixer audioMixer;

    public bool soundOn = true;

    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume2", volume);
    }

    public void ToggleAudio(float returnVolume)
    {
        if (soundOn)
        {
            audioMixer.SetFloat("Volume2", -80);
            soundOn = false;
        }
        else
        {
            audioMixer.SetFloat("Volume2", GameObject.Find("Volume Slider").GetComponent<Slider>().value);
            soundOn = true;
        }
    }

}
