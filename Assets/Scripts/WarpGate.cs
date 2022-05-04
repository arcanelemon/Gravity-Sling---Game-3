using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WarpGate : MonoBehaviour
{
    #region Variables

    int id;
    public int ID => id;

    public bool visible; 
    public bool Visible => visible;

    #endregion Variables

    #region Engine

    void Awake()
    {
        // Generate ID

    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerAction();
        AudioManager.instance.Play("Finished");
    }

    #endregion Engine

    #region Public Methods

    virtual
    public void TriggerAction()
    {
        GameManager.instance.EndLevel();
    }

    #endregion Public Methods
}

