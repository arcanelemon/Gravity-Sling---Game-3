using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using System;
using UnityEngine.Audio;
public class MainMenu : MonoBehaviour
{
    #region Variables

    [SerializeField]
    GameObject introContainer;

    [SerializeField]
    GameObject menuContainer;

    [SerializeField]
    GameObject PlayMenu;

    [SerializeField]
    GameObject HelpMenu;

    [SerializeField]
    GameObject SettingsMenu;
    
    [SerializeField]
    InputActionAsset inputActions;

    InputAction enterAction;

    InputAction nextAction;

    InputAction prevAction;

    PlayerData playerData;

    SettingsManager settingsManager;

    bool started;

    int currId;

    public static MainMenu instance;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Awake()
    {
        var inputMap = inputActions.FindActionMap("UI");
        enterAction = inputMap.FindAction("Start");
        enterAction.performed += OnEnterPress;
        enterAction.Enable();
        nextAction = inputMap.FindAction("Next");
        nextAction.performed += _ => NextTab();
        nextAction.Enable();
        prevAction = inputMap.FindAction("Previous");
        prevAction.performed += _ => PreviousTab();
        prevAction.Enable();

        playerData = SaveManager.LoadPlayerData();
        settingsManager = SettingsMenu.GetComponent<SettingsManager>();
        //AudioManager.instance.Play("MainMenu");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        enterAction.Disable();
        enterAction.Dispose();
        nextAction.Disable();
        nextAction.Dispose();
        prevAction.Disable();
        prevAction.Dispose();
    }

    #endregion Engine

    #region Private Methods

    void OnEnterPress(InputAction.CallbackContext context) 
    {
        if (!started) 
        {
            EnterGame();
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void EnterGame() 
    {
        introContainer.SetActive(false);
        menuContainer.SetActive(true);
        SetTab(currId);
        PlayMenu.GetComponent<PlayManager>().SetCollection(0);
    }

    public void SetTab(int id) 
    {
        PlayMenu.SetActive(false);
        HelpMenu.SetActive(false);
        SettingsMenu.SetActive(false);

        switch (id) 
        {
            case 0:
                PlayMenu.SetActive(true);
                break;
            case 1:
                HelpMenu.SetActive(true);
                break;
            case 2:
                SettingsMenu.SetActive(true);
                settingsManager.PopulateSettings();
                break;
        }
    }

    public void NextTab() 
    {
        currId++;

        if (currId > 2)
            currId = 0;

        SetTab(currId);
    }

    public void PreviousTab() 
    {
        currId--;

        if (currId < 0)
            currId = 2;

        SetTab(currId);
    }

    public void Quit() 
    {
        Application.Quit();
    }

    #endregion Public Methods
}
