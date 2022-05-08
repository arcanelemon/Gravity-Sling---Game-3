using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    #region Variables

    Slider slider;

    public Sound.OutputGroups outputGroup;

    #endregion Variables

    #region Engine

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    #endregion Engine

    #region Public Methods

    public void OnValueChange() 
    {
        float volume = slider.value / slider.maxValue;
        AudioManager.instance.SetOutputGroupVolume(outputGroup, volume);
    }

    public void SetValue(float value) 
    {
        slider.value = value;
    }

    public float GetValue() 
    {
        return slider.value;
    }

    #endregion Public Methods
}
