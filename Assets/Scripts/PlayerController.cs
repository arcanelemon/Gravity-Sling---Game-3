using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float boostForce;

    [SerializeField]
    float attractForce;

    [SerializeField]
    float attractDistance;

    [SerializeField]
    float rotateSpeed;

    [SerializeField]
    float rotateDistance;

    [SerializeField]
    float groundCheckDistance;

    [SerializeField]
    float maxVelocity;

    [SerializeField]
    float gravityConst;

    [SerializeField]
    float gravityMin;

    [SerializeField]
    bool showHelp;

    bool useGravity = true;

    bool grounded;

    bool canBoost = true;

    bool cueJump;

    bool paused;

    bool attract;

    Vector2 moveInputs;

    Vector2 normal;

    Rigidbody2D rigibody;

    GameObject currentBody;

    GameObject secondaryBody;

    GameObject[] gravityBodies;

    FollowCam cam;

    WarpGate warpGate;

    RadialIndicator helpRadialIndicator;

    RadialIndicator attract1RadialIndicator;

    RadialIndicator attract2RadialIndicator;

    #endregion Variables

    #region Input

    [SerializeField]
    InputActionAsset inputActions;

    InputAction moveAction;

    InputAction jumpAction;

    InputAction attractAction;

    InputAction pauseAction;

    InputAction zoomAction;

    void OnMoveChange(InputAction.CallbackContext context)
    {
        moveInputs = context.ReadValue<Vector2>();
    }

    void OnJumpPress(InputAction.CallbackContext context)
    {
        cueJump = true;
    }

    void OnAttractPress(InputAction.CallbackContext context)
    {
        if (!grounded)
        {
            attract = true;
        }
    }
    
    void OnAttractRelease(InputAction.CallbackContext context)
    {
        if (attract)
        {
            Vector3 direction = (currentBody.transform.position - transform.position).normalized;
            rigibody.AddForce(-direction * attractForce);

            attract = false;
        }
    }

    void OnPausePress(InputAction.CallbackContext context)
    {
        if (paused) 
        {
            paused = false;
            GameManager.instance.Resume();
        } else 
        {
            paused = true;
            GameManager.instance.Pause();
        }
    }
    void OnZoomPress(InputAction.CallbackContext context)
    {
        cam.ZoomOut();
    }
    
    void OnZoomRelease(InputAction.CallbackContext context)
    {
        cam.ResetZoom();
    }

    #endregion Input

    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        var inputMap = inputActions.FindActionMap("Player");
        moveAction = inputMap.FindAction("Move");
        jumpAction = inputMap.FindAction("Jump");
        attractAction = inputMap.FindAction("Attract"); 
        pauseAction = inputMap.FindAction("Pause");
        zoomAction = inputMap.FindAction("Zoom");

        moveAction.performed += OnMoveChange;
        moveAction.canceled += OnMoveChange;
        jumpAction.performed += OnJumpPress;
        attractAction.performed += OnAttractPress;
        attractAction.canceled += OnAttractRelease;
        pauseAction.performed += OnPausePress;
        zoomAction.performed += OnZoomPress;
        zoomAction.canceled += OnZoomRelease;

        Enable();

        rigibody = GetComponent<Rigidbody2D>();
        cam = GameObject.FindObjectOfType<FollowCam>();
        gravityBodies = GameObject.FindGameObjectsWithTag("Gravity Body");

        if (showHelp)
        {
            warpGate = GameObject.FindGameObjectWithTag("End").GetComponent<WarpGate>();
            helpRadialIndicator = UIManager.instance.CreateRadialIndicator(RadialIndicator.Type.Help);
        }
    }

     void Update()
    {
        UpdateRadialIcons();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckGround();
        Gravity();

        if (cueJump)
            Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            Vector3 direction = (currentBody.transform.position - transform.position).normalized;
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, -direction);
            transform.rotation = slopeRotation * transform.rotation;

            rigibody.velocity = Vector2.zero;
        }

    }

    void OnDestroy()
    {
        Disable();

        // Unassign inputmoveAction
        jumpAction.Dispose();
        attractAction.Dispose();
        pauseAction.Dispose();
        zoomAction.Dispose();
    }

    #endregion Engine

    #region Private Methods

    void Move()
    {
        if (grounded)
            transform.position = transform.position + transform.right * moveSpeed * moveInputs.x; //Vector3.ProjectOnPlane(transform.position + transform.right * moveSpeed * moveInputs.x, normal);
    }

    void Jump()
    {
        if (grounded)
        {
            // default jump
            AudioManager.instance.Play("Jump");
            rigibody.AddForce((transform.up + transform.right * moveInputs.x).normalized * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(YieldGravity());
        }
        else if (canBoost)
        {
            // boost jump
            rigibody.AddForce(transform.up * boostForce, ForceMode2D.Impulse);
            canBoost = false;

            StartCoroutine(YieldGravity());
        }

        cueJump = false;
    }

    void Gravity()
    {
        if (useGravity)
        {
            float temp = 1e6f;
            Vector3 direction = currentBody == null ? Vector3.zero : (currentBody.transform.position - transform.position).normalized;

            if (true)//!grounded)
            {
                foreach (GameObject gravityBody in gravityBodies)
                {
                    float distance = Vector2.Distance(transform.position, gravityBody.transform.position)
                        - gravityBody.GetComponent<SpriteRenderer>().bounds.size.x / 2;

                    if (distance < temp)
                    {
                        temp = distance;
                        currentBody = gravityBody;
                    }
                }

                foreach (GameObject gravityBody in gravityBodies)
                {
                    float distance = Vector2.Distance(transform.position, gravityBody.transform.position)
                        - gravityBody.GetComponent<SpriteRenderer>().bounds.size.x / 2;

                    if (distance < temp)
                    {
                        temp = distance;
                        secondaryBody = gravityBody;
                    }
                }

                // Pull towards
                rigibody.AddForce(direction * Mathf.Clamp(attract ? attractForce : gravityConst / temp, gravityMin, Mathf.Infinity));
                rigibody.velocity = Vector3.ClampMagnitude(rigibody.velocity, maxVelocity);
            }

            // Rotate
            if (grounded || attract || temp < rotateDistance)
            {
                Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, -direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, slopeRotation * transform.rotation, rotateSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void CheckGround()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, LayerMask.GetMask("Ground"));
        if (!grounded && hit.collider != null)
        {

            attract = false;
            grounded = true;
            canBoost = true;
            normal = hit.normal;
        } else if (hit.collider == null)
        {
            grounded = false;
        }

        if (hit.point != Vector2.zero)
            Debug.DrawLine(transform.position, hit.point, Color.blue, 0.1f);
    }

    IEnumerator YieldGravity() 
    {
        useGravity = false;
        yield return new WaitForSeconds(0.2f);
        useGravity = true;
    }

    void UpdateRadialIcons() 
    {
        if (showHelp) 
        {
            if (warpGate.Visible && helpRadialIndicator.Visible)
                helpRadialIndicator.SetVisibility(false);
            else if (!warpGate.Visible && !helpRadialIndicator.Visible)
                helpRadialIndicator.SetVisibility(true);

            helpRadialIndicator.SetAngle(Vector2.SignedAngle(Vector2.up, (warpGate.transform.position - transform.position).normalized));
        }

        if (attract1RadialIndicator != null) 
        {
                
        }

        if (attract2RadialIndicator != null) 
        {
            
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void Enable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        attractAction.Enable();
        pauseAction.Enable();
        zoomAction.Enable();
    }

    public void Disable() 
    {
        moveAction.Disable();
        jumpAction.Disable();
        attractAction.Disable();
        pauseAction.Disable();
        zoomAction.Disable();
    }

    #endregion Public Methods
}
