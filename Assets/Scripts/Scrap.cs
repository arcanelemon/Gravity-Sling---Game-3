using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class Scrap : MonoBehaviour
{
    #region Variables

    [SerializeField]
    int value;
    public int Value => value;

    int id;
    public int ID => id;

    #endregion Variables

    #region Engine

    void Awake()
    {
        // Generate ID
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerAction();
        AudioManager.instance.Play("Scrap_Pickup");
        Destroy(gameObject);
    }

    #endregion Engine

    #region Public Methods

    virtual
    public void TriggerAction() 
    {
        GameManager.instance.IncrementScrap(value);
    }

    #endregion Public Methods
}
