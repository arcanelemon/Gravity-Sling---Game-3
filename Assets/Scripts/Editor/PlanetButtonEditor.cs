using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[CustomEditor(typeof(PlanetButton))]
public class PlanetButtonEditor : UnityEditor.UI.ButtonEditor
{


    public override void OnInspectorGUI()
    {

        PlanetButton component = (PlanetButton)target;

        base.OnInspectorGUI();
        component.image = (Image)EditorGUILayout.ObjectField("Image", component.image, typeof(Image), true);
        component.textUGUI = (TextMeshProUGUI)EditorGUILayout.ObjectField("TextMeshProUGUI", component.textUGUI, typeof(TextMeshProUGUI), true);
    }
}
