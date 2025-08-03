using UnityEngine;

public class SoundUtils
{
    public static void PlayWithRandomPitch(AudioSource source, AudioClip clip, float volume = 1f)
    {
        source.pitch = Random.Range(0.85f, 1.15f);
        source.PlayOneShot(clip, volume);
    }
        
}