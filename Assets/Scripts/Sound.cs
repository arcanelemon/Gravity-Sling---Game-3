using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    [Range(0.1f, 3)]
    public float pitch;

    [Range(0, 1)]
    public float volume;

    public bool looping;

    public OutputGroups outputGroup;

    public AudioClip audioClip;

    public AudioSource audioSource;

    public enum OutputGroups 
    { 
        Master,
        Music,
        FX,
        Voice,
    }
}
