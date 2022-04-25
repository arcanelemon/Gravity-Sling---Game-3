using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Transform target;

    [SerializeField]
    float speed;

    [SerializeField]
    float standardDistance;

    [SerializeField]
    float zoomDistance;

    [SerializeField]
    float zoomTime;

    [SerializeField]
    float resetTime;

    bool zoom;

    bool reset;

    Camera cam;
    public Camera Cam => cam;

    #endregion Variables

    #region Engine

    private void Start()
    {
        cam = GetComponent<Camera>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -standardDistance);
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;

        if (zoom || reset) 
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, zoom ? -zoomDistance : -standardDistance), (zoom ? zoomTime : resetTime) * Time.deltaTime);
        }
    }

    #endregion Engine

    #region Public Methods

    public void ResetZoom() 
    {
        zoom = false;
        reset = true;
    }

    public void ZoomOut() 
    {
        zoom = true;
        reset = false;
    }

    #endregion Public Methods
}
