using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    Transform[] children;

    #region Engine

    // Start is called before the first frame update
    void Awake()
    {
        // only return top children
        children = GetComponentsInChildren<Transform>();
        List<Transform> temp = new List<Transform>();

        foreach(Transform child in children) 
        {
            if (child.parent == transform)
                temp.Add(child);
        }

        children = temp.ToArray();
    }

    #endregion Engine

    #region Public Methods

    public void PopulateSettings() 
    {
        // load config
        ConfigData data = SaveManager.LoadConfigData();

        // populate components
        foreach (Transform child in children)
        {
            AudioSlider audioSlider = child.GetComponent<AudioSlider>();
            if (audioSlider != null)
            {
                float value = 1;
                if (data != null)
                {
                    switch (audioSlider.outputGroup)
                    {
                        case (Sound.OutputGroups.Master):
                            value = data.masterVolume;
                            break;
                        case (Sound.OutputGroups.Music):
                            value = data.musicVolume;
                            break;
                        case (Sound.OutputGroups.FX):
                            value = data.fxVolume;
                            break;
                    }
                }

                audioSlider.SetValue(value);
                continue;
            }

            DisplayModeDropdown displayDropdown = child.GetComponent<DisplayModeDropdown>();
            if (displayDropdown != null)
            {
                string displayMode;
                if (data != null)
                    displayMode = data.displayMode.ToString();
                else
                    displayMode = DisplayMode.FromFullscreenMode(Screen.fullScreenMode).ToString();

                displayDropdown.SetDropdownValue(displayMode);
                continue;
            }

            ResolutionDropdown resolutionDropdown = child.GetComponent<ResolutionDropdown>();
            if (resolutionDropdown != null)
            {
                string resolution;
                if (data != null)
                    resolution = data.displayMode.ToString();
                else
                    resolution = Screen.currentResolution.ToString();

                resolutionDropdown.SetDropdownValue(resolution);
                continue;
            }
        }
    }

    public void SaveSettings() 
    {
        Resolution resolution = Screen.currentResolution;
        DisplayMode.Mode displayMode = DisplayMode.Mode.Fullscreen;

        float masterVolume = 1;
        float musicVolume = 1;
        float fxVolume = 1;

        // pull from components
        foreach (Transform child in children)
        {
            AudioSlider audioSlider = child.GetComponent<AudioSlider>();
            if (audioSlider != null)
            {
                switch (audioSlider.outputGroup)
                {
                    case (Sound.OutputGroups.Master):
                        masterVolume = audioSlider.GetValue();
                        break;
                    case (Sound.OutputGroups.Music):
                        musicVolume = audioSlider.GetValue();
                        break;
                    case (Sound.OutputGroups.FX):
                        fxVolume = audioSlider.GetValue();
                        break;
                }

                continue;
            }

            DisplayModeDropdown displayDropdown = child.GetComponent<DisplayModeDropdown>();
            if (displayDropdown != null)
            {
                displayMode = displayDropdown.GetDropdownValue();
            }

            ResolutionDropdown resolutionDropdown = child.GetComponent<ResolutionDropdown>();
            if (resolutionDropdown != null)
            {
                resolution = resolutionDropdown.GetDropdownValue();
            }
        }

        // Save Data
        SaveManager.SaveConfigData(new ConfigData(resolution.width + "x" + resolution.height, displayMode, new float[] { masterVolume, musicVolume, fxVolume }));
    }

    #endregion Public Methods
}
