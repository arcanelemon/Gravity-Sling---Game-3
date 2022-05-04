using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Startup : MonoBehaviour
{
    IEnumerator Start()
    {
        ConfigData configData = SaveManager.LoadConfigData();
        if (configData != null)
        {
            Screen.SetResolution(configData.resolution.width, configData.resolution.height, Screen.fullScreen);
            Screen.fullScreenMode = DisplayMode.ToFullscreenMode(configData.displayMode);
        }

        SplashScreen.Begin();
        while (!SplashScreen.isFinished)
        {
            SplashScreen.Draw();
            yield return null;
        }
    }
}
