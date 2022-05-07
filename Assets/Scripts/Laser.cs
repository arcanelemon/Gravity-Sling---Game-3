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

    [Space(10)]

    [SerializeField]
    float fireTime;

    [SerializeField]
    float warmTime;

    [SerializeField]
    Vector2 waitTime;

    Animator animator;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (playOnAwake) 
        {
            StartCoroutine(YieldStartTime());
        } else 
        {
            StartCoroutine(YieldWaitTime());
        }
    }

    #endregion Engine

    #region Private Methods

    IEnumerator YieldStartTime()
    {
        yield return new WaitForSeconds(delay);
        WarmLaser();
    }

    IEnumerator YieldFireTime()
    {
        yield return new WaitForSeconds(fireTime);
        ResetLaser();
    }

    IEnumerator YieldWaitTime()
    {
        yield return new WaitForSeconds(Random.Range(waitTime.x, waitTime.y));
        WarmLaser();
    }

    IEnumerator YieldWarmTime()
    {
        yield return new WaitForSeconds(warmTime);
        FireLaser();
    }

    private void WarmLaser() 
    {
        animator.SetBool("Warm", true);
        StartCoroutine(YieldWarmTime());
    }

    private void FireLaser() 
    {
        animator.SetBool("Warm", false);
        animator.SetBool("Fire", true);

        StartCoroutine(YieldFireTime());
    }

    private void ResetLaser()
    {
        animator.SetBool("Fire", false);
        StartCoroutine(YieldWaitTime());
    }

    #endregion Private Methods

    #region Public Methods

    #endregion Public Methods
}
