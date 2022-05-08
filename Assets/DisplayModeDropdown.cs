using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayModeDropdown : MonoBehaviour
{
    #region Variables

    Dropdown dropdown;

    #endregion Variables

    #region Engine

    // Start is called before the first frame update
    void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(Enum.GetNames(typeof(DisplayMode.Mode))));
    }

    #endregion Engine

    #region Public Methods

    public void SetDropdownValue(int i) 
    {
        dropdown.value = i;
        dropdown.RefreshShownValue();
        UpdateDisplayMode();
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

    public DisplayMode.Mode GetDropdownValue() 
    {
        return DisplayMode.FromString(dropdown.options[dropdown.value].ToString());
    }

    public void UpdateDisplayMode() 
    {
        FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;

        switch (dropdown.value)
        {
            case 1:
                mode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                mode = FullScreenMode.Windowed;
                break;
        }

        Screen.fullScreenMode = mode;
    }

    #endregion Public Methods
}
