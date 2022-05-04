using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Variables

    [SerializeField]
    bool playOnAwake;

    [SerializeField]
    float delay;

    [SerializeField]
    bool randomDelay;

    [Space(10)]

    [SerializeField]
    float fireTime;

    [SerializeField]
    Vector2 waitTime;

    Animator animator;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        
    }

    #endregion Engine

    #region Private Methods

    //IEnumerator YieldStartTime() 
    //{
        
    //    yield return new WaitForSeconds()
    //}

    //IEnumerator YieldFireTime() 
    //{
    
    //}

    //IEnumerator YieldWaitTime() 
    //{
    
    //}

    private void FireLaser() 
    {
        animator.SetBool("Fire", true);
    }

    private void ResetLaser()
    {
        animator.SetBool("Fire", false);
    }

    #endregion Private Methods

    #region Public Methods

    #endregion Public Methods
}
