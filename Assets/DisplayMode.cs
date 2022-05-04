using System;
using UnityEngine;

static public class DisplayMode
{ 
    #region Variables

    public enum Mode 
    {
        Fullscreen,
        Borderless,
        Windowed
    }

    #endregion Variables

    #region Public Methods

    static public Mode FromString(string value) 
    {
        Mode result;
        Enum.TryParse(value, out result);
        return result;
    }

    static public Mode FromFullscreenMode(FullScreenMode fullScreenMode) 
    {
        Mode mode = Mode.Fullscreen;

        switch (fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow:
                mode = Mode.Borderless;
                break;
            case FullScreenMode.MaximizedWindow:
                mode = Mode.Windowed;
                break;
        }

        return mode;
    }

    static public FullScreenMode ToFullscreenMode(Mode mode) 
    {
        FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;

        switch (mode) 
        {
            case Mode.Borderless:
                fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case Mode.Windowed:
                fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }

        return fullScreenMode;
    }

    #endregion Public Methods
}
