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

    [SerializeField]
    Animator[] tabAnims;

    InputAction enterAction;

    InputAction nextAction;

    InputAction prevAction;

    PlayerData playerData;

    PlayManager playManager;

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
        nextAction.performed += NextTab;
        nextAction.Enable();
        prevAction = inputMap.FindAction("Previous");
        prevAction.performed += PreviousTab;
        prevAction.Enable();

        playerData = SaveManager.LoadPlayerData();
        playManager = PlayMenu.GetComponent<PlayManager>();
        settingsManager = SettingsMenu.GetComponent<SettingsManager>();
        //AudioManager.instance.Play("MainMenu");

        ConfigData configData = SaveManager.LoadConfigData();
        if (configData != null)
        {
            string[] resolution = configData.resolution.Split('x');

            Screen.SetResolution(Int32.Parse(resolution[0]), Int32.Parse(resolution[1]), Screen.fullScreen);
            Screen.fullScreenMode = DisplayMode.ToFullscreenMode(configData.displayMode);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        introContainer = null;
        menuContainer = null;
        PlayMenu = null;
        HelpMenu = null;
        SettingsMenu = null;
        Dispose();
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
    }

    public void SetTab(int id) 
    {
        PlayMenu.SetActive(false);
        HelpMenu.SetActive(false);
        SettingsMenu.SetActive(false);

        foreach (Animator anim in tabAnims)
            anim.Play("Inactive");

        switch (id) 
        {
            case 0:
                tabAnims[0].Play("Active");
                PlayMenu.SetActive(true);
                playManager.SetCollection(0);
                break;
            case 1:
                tabAnims[1].Play("Active");
                HelpMenu.SetActive(true);
                break;
            case 2:
                tabAnims[2].Play("Active");
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

    public void NextTab(InputAction.CallbackContext context) 
    {
        NextTab();
    }

    public void PreviousTab(InputAction.CallbackContext context) 
    {
        PreviousTab();
    }

    public void Quit() 
    {
        Application.Quit();
    }

    public void Dispose() 
    {
        enterAction.Disable();
        enterAction.performed -= OnEnterPress;
        enterAction.Dispose();
        nextAction.Disable();
        nextAction.performed -= NextTab;
        nextAction.Dispose();
        prevAction.Disable();
        prevAction.performed -= PreviousTab;
        prevAction.Dispose();
    }

    #endregion Public Methods
}
