using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : MonoBehaviour
{
    void Awake()
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
