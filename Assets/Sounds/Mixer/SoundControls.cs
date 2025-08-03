using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundControls : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    public bool sfxTurned;
    public bool musicTurned;

    private void OnEnable()
    {
        mixer.SetFloat("SFX", -19.26f);
        mixer.SetFloat("Music", 0f);
    }

    public void SfxButton()
    {
        if (sfxTurned)
        {
            mixer.SetFloat("SFX", -80f);
        }
        else
        {
            mixer.SetFloat("SFX", -19.26f);
        }

        sfxTurned = !sfxTurned;
    }
    
    public void MusicButton()
    {
        if (musicTurned)
        {
            mixer.SetFloat("Music", -80f);
        }
        else
        {
            mixer.SetFloat("Music", 0f);
        }

        musicTurned = !musicTurned;
    }
}
