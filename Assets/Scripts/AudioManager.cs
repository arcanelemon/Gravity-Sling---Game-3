using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) 
        {
            Destroy(gameObject);
        }

        instance = this;

        foreach(Sound sound in sounds) 
        {
            AddAudioSource(sound);
        }
    }

    void Save() 
    {
    
    }

    string FormatOutputGroupToVolume(Sound.OutputGroups outputGroup) 
    {
        return outputGroup.ToString() + " Volume";
    }

    void AddAudioSource(Sound sound) 
    {
        sound.audioSource = gameObject.AddComponent<AudioSource>();
        sound.audioSource.clip = sound.audioClip;
        sound.audioSource.volume = sound.volume;
        sound.audioSource.pitch = sound.pitch;
        sound.audioSource.loop = sound.looping;
        sound.audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(sound.outputGroup.ToString())[0];
    }

    public void Play(string name) 
    {
        // search for audiosource
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) 
        {
            Debug.LogWarning("Sound: " + name + " could not be located.");
            return;
        }

        if (sound.audioSource.isPlaying) 
        {
            AddAudioSource(sound);
        }

        sound.audioSource.Play();
    }

    public void Stop(string name) 
    {
        // search for audiosource
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be located.");
            return;
        }

        if (sound.audioSource.isPlaying)
        {
            sound.audioSource.Stop();
        }
    }

    public void MuteOutputGroup(Sound.OutputGroups group) 
    {
        audioMixer.SetFloat(group.ToString(), 0f);

        Save();
    }

    public void SetOutputGroupVolume(Sound.OutputGroups group, float volume)
    {
        Mathf.Clamp(volume, 0, 1);
        audioMixer.SetFloat(FormatOutputGroupToVolume(group), Mathf.Max(Mathf.Log10(volume) * 20, -80));

        Save();
    }

    public float GetOutputGroupVolume(Sound.OutputGroups group) 
    {
        float volume;
        audioMixer.GetFloat(FormatOutputGroupToVolume(group), out volume);

        return Mathf.Pow(10, volume) / 20;
    }
}
