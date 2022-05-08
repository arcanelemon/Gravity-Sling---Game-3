using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour
{
    #region Variables

    Resolution[] resolutions;

    Dropdown dropdown;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Awake()
    {
        resolutions = Screen.resolutions;
        dropdown = GetComponent<Dropdown>();

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    #endregion Engine

    #region Public Methods

    public void SetDropdownValue(int i)
    {
        dropdown.value = i;
        dropdown.RefreshShownValue();
        UpdateResolution();
    }

    public void SetDropdownValue(string name) 
    {
        int value = 0;
        int index = 0;
        foreach (Dropdown.OptionData option in dropdown.options)
        {
            if (name == option.ToString())
                value = index;

            index++;
        }

        dropdown.value = value;
        dropdown.RefreshShownValue();
    }
    public Resolution GetDropdownValue()
    {
        return Screen.resolutions[dropdown.value];
    }

    public void UpdateResolution() 
    {
        int index = dropdown.value;
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

    #endregion Public Methods
}
