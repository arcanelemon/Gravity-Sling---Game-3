using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Transform anchor;

    [SerializeField]
    float speed;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 15 * Time.deltaTime);
        transform.RotateAround(anchor.position, transform.forward, speed * Time.deltaTime);
    }

    #endregion Engine
}
