using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCollection : MonoBehaviour
{
    #region Variables

    [SerializeField]
    string systemName;
    public string SystemName => systemName;

    [SerializeField]
    int scrapRequirement;
    public int ScrapRequirement => scrapRequirement;

    #endregion Variables
}
