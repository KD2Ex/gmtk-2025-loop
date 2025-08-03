using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundControls : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private GameObject sfxButton;
    [SerializeField] private GameObject mscButton;
    
    public bool sfxTurned;
    public bool musicTurned;

    private Animator sfxAnim;
    private Animator mscAnim;

    private float ogSFX;
    private float ogMusic;
    
    

    private void Awake()
    {
        sfxAnim = sfxButton.GetComponent<Animator>();
        mscAnim = mscButton.GetComponent<Animator>();
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
            sfxAnim.SetTrigger("Unpressed");
            mixer.SetFloat("SFX", -80f);
        }
        else
        {
            sfxAnim.SetTrigger("Unpressed");
            mixer.SetFloat("SFX", ogSFX);
        }
        

        sfxTurned = !sfxTurned;
    }
    
    public void MusicButton()
    {
        if (musicTurned)
        {
            mscAnim.SetTrigger("Unpressed");
            mixer.SetFloat("Music", -80f);
        }
        else
        {
            mscAnim.SetTrigger("Unpressed");
            mixer.SetFloat("Music", ogMusic);
        }

        musicTurned = !musicTurned;
    }
}
