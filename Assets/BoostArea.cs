using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostArea : MonoBehaviour
{
    #region Variables

    [SerializeField]
    float boostForce;

    #endregion Variables

    #region Engine

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag is "Player") 
        {
            Debug.Log("Hit");
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(transform.right * boostForce * 100);
        }
    }

    #endregion Engine
}
