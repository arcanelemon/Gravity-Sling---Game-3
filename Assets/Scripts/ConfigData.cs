using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigData
{
    public string resolution;

    public DisplayMode.Mode displayMode;

    public float masterVolume;

    public float musicVolume;

    public float fxVolume;

    public ConfigData(string resolution, DisplayMode.Mode displayMode, float[] outputVolume) 
    {
        this.resolution = resolution;
        this.displayMode = displayMode;
        masterVolume = outputVolume[0];
        musicVolume = outputVolume[1];
        fxVolume = outputVolume[2];
    }
}
