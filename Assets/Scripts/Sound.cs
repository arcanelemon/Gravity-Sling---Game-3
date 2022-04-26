using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name = "New Sound";

    [Range(0.1f, 3)]
    public float pitch = 1;

    [Range(0, 1)]
    public float volume = 1;

    public bool looping;

    public OutputGroups outputGroup;

    public AudioClip audioClip;

    [HideInInspector]
    public AudioSource audioSource;

    public enum OutputGroups 
    { 
        Master,
        Music,
        FX,
        Voice,
    }
}
