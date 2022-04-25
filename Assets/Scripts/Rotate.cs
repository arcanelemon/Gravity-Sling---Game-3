using UnityEngine;

public class Rotate : MonoBehaviour
{
    #region Variables

    [SerializeField]
    float speed;

    #endregion Variables
    
    #region Engine

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }

    #endregion Engine
}
