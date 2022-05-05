using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Planet", menuName = "Data/Planet")]
public class PlanetData : ScriptableObject
{
    [SerializeField]
    Sprite sprite;
    public Sprite Sprite => sprite;

    [SerializeField]
    string name;
    public string Name => name;
}
