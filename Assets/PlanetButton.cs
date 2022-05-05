using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[CustomEditor(typeof(PlanetButton))]
public class PlanetButton : Button
{
    #region Variables

    [SerializeField]
    Image image;

    [SerializeField]
    TextMeshProUGUI textUGUI;

    PlayManager manager;

    #endregion Variables

    #region Private Methods

    protected override void Start()
    {
        base.Start();
        manager = FindObjectOfType<PlayManager>();
    }

    //public override void OnInspectorGUI()
    //{
    //    MenuButton targetMenuButton = (MenuButton)target;

    //    targetMenuButton.acceptsPointerInput = EditorGUILayout.Toggle("Accepts pointer input", targetMenuButton.acceptsPointerInput);

    //    base.OnInspectorGUI;
    //}

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
