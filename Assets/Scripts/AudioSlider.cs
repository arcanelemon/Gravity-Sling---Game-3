using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{ 
    #region Variables

    public Sound.OutputGroups outputGroup;

    #endregion Variables

    #region Engine

    void Start()
    {
        GetComponent<Slider>().value = AudioManager.instance.GetOutputGroupVolume(outputGroup);
    }

    #endregion Engine

    #region Public Methods

    public void OnValueChange(float volume) 
    {
        AudioManager.instance.SetOutputGroupVolume(outputGroup, volume);
    }

    #endregion Public Methods
}
