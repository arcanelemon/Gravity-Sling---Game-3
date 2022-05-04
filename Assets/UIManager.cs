using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    GameObject radialIndicatorPrefab;

    public static UIManager instance;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion Engine

    #region Private Methods

    #endregion Private Methods

    #region Public Methods

    public RadialIndicator CreateRadialIndicator(RadialIndicator.Type type)
    {
        RadialIndicator radialIndicator = Instantiate(radialIndicatorPrefab, transform).GetComponent<RadialIndicator>();
        radialIndicator.SetSpriteFromType(type);
        return radialIndicator;
    }

    #endregion Public Methods
}
