using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Engine

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag is "Player")
            GameManager.instance.ResetGame();
    }

    #endregion Engine
}
