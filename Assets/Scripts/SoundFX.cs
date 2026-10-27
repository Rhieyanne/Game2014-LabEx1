using UnityEngine;


public static class SoundFX
{
    static AudioSource source;

    public static void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;  // no sound set, skip it

        if (source == null)
        {
            GameObject soundObject = new GameObject("SoundFX");
            Object.DontDestroyOnLoad(soundObject);
            source = soundObject.AddComponent<AudioSource>();
        }

        source.PlayOneShot(clip, volume);
    }
}
