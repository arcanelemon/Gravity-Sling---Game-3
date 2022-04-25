using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigScrap : Scrap
{

    #region Private Methods

    override
    public void TriggerAction()
    {
        base.TriggerAction();
        GameManager.instance.EndLevel();
    }

    #endregion Private Methods
}
