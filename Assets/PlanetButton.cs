using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;


public class PlanetButton : Button
{
    #region Variables

    [SerializeField]
    public Image image;

    [SerializeField]
    public TextMeshProUGUI textUGUI;

    PlayManager manager;

    #endregion Variables

    #region Private Methods

    protected override void Start()
    {
        base.Start();
        manager = FindObjectOfType<PlayManager>();
    }

        #endregion Private Methods

        #region Public Methods

        public void UpdateButton(PlanetData data) 
    {
        onClick.RemoveAllListeners();
        onClick.AddListener(delegate { manager.LoadLevel(data.Name); });

        image.sprite = data.Sprite;
        textUGUI.text = data.Name;
    }

    #endregion Public Methods
}
