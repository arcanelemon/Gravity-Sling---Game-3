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

    Vector3 oldNormal;

    Rigidbody2D rigibody;

    GameObject currentBody;

    GameObject secondaryBody;

    GameObject attractBody;

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

    InputAction attract1Action;

    InputAction attract2Action;

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

    void OnAttract1Press(InputAction.CallbackContext context)
    {
        if (!grounded)
        {
            attract = true;
            attractBody = currentBody;
        }
    }
    
    void OnAttract1Release(InputAction.CallbackContext context)
    {
        if (attract && currentBody != null)
        {
            Vector3 direction = (currentBody.transform.position - transform.position).normalized;
            rigibody.AddForce(-direction * attractForce);

            attract = false;
        }
    }
    
    void OnAttract2Press(InputAction.CallbackContext context)
    {
        if (!grounded)
        {
            attract = true;
            attractBody = secondaryBody;
        }
    }
    
    void OnAttract2Release(InputAction.CallbackContext context)
    {
        if (attract && secondaryBody != null)
        {
            Vector3 direction = (secondaryBody.transform.position - transform.position).normalized;
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
        attract1Action = inputMap.FindAction("Attract1"); 
        attract2Action = inputMap.FindAction("Attract2"); 
        pauseAction = inputMap.FindAction("Pause");
        zoomAction = inputMap.FindAction("Zoom");

        moveAction.performed += OnMoveChange;
        moveAction.canceled += OnMoveChange;
        jumpAction.performed += OnJumpPress;
        attract1Action.performed += OnAttract1Press;
        attract1Action.canceled += OnAttract1Release;
        attract2Action.performed += OnAttract2Press;
        attract2Action.canceled += OnAttract2Release;
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

        attract1RadialIndicator = UIManager.instance.CreateRadialIndicator(RadialIndicator.Type.Attract1);
        attract2RadialIndicator = UIManager.instance.CreateRadialIndicator(RadialIndicator.Type.Attract2);
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
        attract1Action.Dispose();
        pauseAction.Dispose();
        zoomAction.Dispose();
        attract2Action.Dispose();
    }

    #endregion Engine

    #region Private Methods

    void Move()
    {
        if (grounded)
        {
            transform.position = transform.position + transform.right * moveInputs.x * moveSpeed * Time.deltaTime;
        }
    }

    void Jump()
    {
        if (grounded)
        {
            // default jump
            AudioManager.instance.Play("Jump");
            rigibody.AddForce((2*transform.up + transform.right * moveInputs.x).normalized * jumpForce, ForceMode2D.Impulse);
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

            temp = 1e6f;
            foreach (GameObject gravityBody in gravityBodies)
            {
                float distance = Vector2.Distance(transform.position, gravityBody.transform.position)
                    - gravityBody.GetComponent<SpriteRenderer>().bounds.size.x / 2;

                if (distance < temp && gravityBody != currentBody)
                {
                    temp = distance;
                    secondaryBody = gravityBody;
                }
            }

            if(attract && Vector2.Distance(transform.position, attractBody.transform.position) > attractDistance) 
            {
                attractBody = null;
                attract = false;
            }

            // Pull towards
            rigibody.AddForce( attract ? ((attractBody.transform.position - transform.position).normalized * attractForce ) : (direction * Mathf.Clamp(gravityConst / temp, gravityMin, Mathf.Infinity)));
            rigibody.velocity = Vector3.ClampMagnitude(rigibody.velocity, maxVelocity);

            // Rotate
            if (attract || (temp < rotateDistance && !grounded))
            {
                Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, -direction);
                Quaternion targetRotation = Quaternion.Lerp(transform.rotation, slopeRotation * transform.rotation, rotateSpeed * Time.fixedDeltaTime);
                if (Mathf.Abs(transform.rotation.z - targetRotation.z) > .001f) 
                {
                    transform.rotation = targetRotation;
                    //transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z);
                }
            }
        }
    }

    void CheckGround()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            //if the current goTransform.up.y value has passed the threshold test  
            if (oldNormal.y >= transform.up.y + .009f || oldNormal.y <= transform.up.y - .009f)
            {
                //set the up vector to match the normal of the ray's collision  
                transform.up = hit.normal;
            }

            //store the current hit.normal inside the oldNormal  
            oldNormal = hit.normal;

            attract = false;
            grounded = true;
            canBoost = true;
        } else
        {
            grounded = false;
        }

        Debug.DrawRay(transform.position, -transform.up * groundCheckDistance, Color.blue, 0.1f);
    }

    IEnumerator YieldGravity() 
    {
        useGravity = false;
        yield return new WaitForSeconds(0.2f);
        useGravity = true;
    }

    void UpdateRadialIcons() 
    {
        if (showHelp && helpRadialIndicator != null) 
        {
            if (warpGate.Visible && helpRadialIndicator.Visible)
                helpRadialIndicator.SetVisibility(false);
            else if (!warpGate.Visible && !helpRadialIndicator.Visible)
                helpRadialIndicator.SetVisibility(true);

            helpRadialIndicator.SetAngle(Vector2.SignedAngle(Vector2.up, (warpGate.transform.position - transform.position).normalized));
        }

        if (currentBody != null) 
        {
            attract1RadialIndicator.SetAngle(Vector2.SignedAngle(Vector2.up, (currentBody.transform.position - transform.position).normalized));

            if (attract1RadialIndicator.Visible && (Vector2.Distance(currentBody.transform.position, transform.position) > attractDistance || grounded))
            {
                attract1RadialIndicator.SetVisibility(false);
            }
            else if (!attract1RadialIndicator.Visible && (Vector2.Distance(currentBody.transform.position, transform.position) < attractDistance && !grounded))
            {
                attract1RadialIndicator.SetVisibility(true);
            }
        }

        if (secondaryBody != null) 
        {
            attract2RadialIndicator.SetAngle(Vector2.SignedAngle(Vector2.up, (secondaryBody.transform.position - transform.position).normalized));


            if (attract2RadialIndicator.Visible && (Vector2.Distance(secondaryBody.transform.position, transform.position) > attractDistance || grounded))
            {
                attract2RadialIndicator.SetVisibility(false);
            } else if (!attract2RadialIndicator.Visible && (Vector2.Distance(secondaryBody.transform.position, transform.position) < attractDistance && !grounded)) 
            {
                attract2RadialIndicator.SetVisibility(true);
            }
        }
    }

    #endregion Private Methods

    #region Public Methods

    public void Enable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        attract1Action.Enable();
        attract2Action.Enable();
        pauseAction.Enable();
        zoomAction.Enable();
    }

    public void Disable() 
    {
        moveAction.Disable();
        jumpAction.Disable();
        attract1Action.Disable();
        attract2Action.Disable();
        pauseAction.Disable();
        zoomAction.Disable();
    }

    #endregion Public Methods
}
