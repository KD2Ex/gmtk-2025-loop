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

    [SerializeField] private Color offColor;
    [SerializeField] private Color onColor;
    
    public bool sfxTurned;
    public bool musicTurned;

    private Animator sfxAnim;
    private Animator mscAnim;

    private float ogSFX;
    private float ogMusic;

    private Image buttonImage;
    private Image musicButtonImage;
    
    private TMP_Text sfxButtonText;
    private TMP_Text musicButtonText;

    private Color ogFontColor;

    private void Awake()
    {
        sfxAnim = sfxButton.GetComponent<Animator>();
        mscAnim = mscButton.GetComponent<Animator>();
        mixer.GetFloat("SFX", out var sfx);
        mixer.GetFloat("Music", out var music);
        ogSFX = sfx;
        ogMusic = music;
        
        buttonImage = sfxButton.GetComponent<Image>();
        musicButtonImage = mscButton.GetComponent<Image>();
        
        sfxButtonText = sfxButton.GetComponentInChildren<TMP_Text>();
        musicButtonText = mscButton.GetComponentInChildren<TMP_Text>();
        
        ogFontColor = sfxButtonText.color;
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
            //sfxAnim.SetTrigger("Unpressed");
            buttonImage.color = offColor;
            sfxButtonText.color = onColor;
            mixer.SetFloat("SFX", -80f);
        }
        else
        {
            buttonImage.color = onColor;
            sfxButtonText.color = ogFontColor;
            //sfxAnim.SetTrigger("Unpressed");
            mixer.SetFloat("SFX", ogSFX);
        }
        

        sfxTurned = !sfxTurned;
    }
    
    public void MusicButton()
    {
        if (musicTurned)
        {
            musicButtonImage.color = offColor;
            musicButtonText.color = onColor;
            mixer.SetFloat("Music", -80f);
        }
        else
        {
            musicButtonImage.color = onColor;
            musicButtonText.color = ogFontColor;
            mixer.SetFloat("Music", ogMusic);
        }

        musicTurned = !musicTurned;
    }
}
