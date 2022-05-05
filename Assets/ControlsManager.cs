using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    GameObject gamepad;

    [SerializeField]
    GameObject keyboard;

    #endregion Variables

    #region Public Methods

    public void ShowGamepad()
    {
        gamepad.SetActive(true);
        keyboard.SetActive(false);
    }

    public void ShowKeyboard()
    {
        gamepad.SetActive(false);
        keyboard.SetActive(true);
    }

    #endregion Public Methods
}
