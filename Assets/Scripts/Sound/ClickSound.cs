using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ClickSound : MonoBehaviour
{

    public AudioClip soundClip;

    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } }


    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = soundClip;
        source.playOnAwake = false;
        //other settings


        button.onClick.AddListener(() => PlaySound());
    }

    void PlaySound() {
        source.PlayOneShot(soundClip);
        AwakeNow();
    }

    void AwakeNow()
    {
        if (FindObjectsOfType(typeof(Sound)).Length > 1)
        {
            Destroy(gameObject);
            return; // don't allow code to continue executing since we're destroy this "extra" copy.
        }

        var _audio = this.GetComponent<AudioSource>();
        if (_audio.clip != null && _audio.time == 0)
        { // check if audio clip assigned and only do this if it hasn't started playing yet (position == 0)
            _audio.Play();
        }
    }


}
