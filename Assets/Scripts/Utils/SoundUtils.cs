using UnityEngine;

public class SoundUtils
{
    public static void PlayWithRandomPitch(AudioSource source, AudioClip clip)
    {
        source.pitch = Random.Range(0.85f, 1.15f);
        source.PlayOneShot(clip, 1f);
    }
        
}