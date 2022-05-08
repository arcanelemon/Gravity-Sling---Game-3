using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryRotationIcon : MonoBehaviour
{ 
    #region Variables

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, -transform.parent.rotation.z);
    }

    #endregion Engine

    #region Private Methods

    #endregion Private Methods

    #region Public Methods

    #endregion Public Methods
}
