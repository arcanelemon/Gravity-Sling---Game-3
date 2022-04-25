using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int kills;

    public SaveData(int kills) 
    {
        this.kills = kills;
    }
}
