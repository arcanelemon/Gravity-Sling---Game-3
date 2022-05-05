using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Planet System", menuName = "Data/PlanetSystem")]
public class PlanetSystemData : ScriptableObject
{
    [SerializeField]
    string systemName;
    public string SystemName => systemName;

    [SerializeField]
    int scrapRequirement;
    public int ScrapRequirement => scrapRequirement;

    [SerializeField]
    PlanetData[] planets;
    public PlanetData[] Planets => planets;
}
