using UnityEngine;
using UnityEngine.Audio;

public class SoundControls : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    public bool sfxTurned;
    public bool musicTurned;

    private float ogSFX;
    private float ogMusic;

    private void Awake()
    {
        mixer.GetFloat("SFX", out var sfx);
        mixer.GetFloat("Music", out var music);
        ogSFX = sfx;
        ogMusic = music;
    }

    private void OnEnable()
    {
        mixer.SetFloat("SFX", ogSFX);
        mixer.SetFloat("Music", ogMusic);
    }

    public void SfxButton()
    {
        if (sfxTurned)
        {
            mixer.SetFloat("SFX", -80f);
        }
        else
        {
            mixer.SetFloat("SFX", ogSFX);
            
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
            mixer.SetFloat("Music", ogMusic);
        }

        musicTurned = !musicTurned;
    }
}
