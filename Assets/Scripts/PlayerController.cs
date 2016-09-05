using UnityEngine;
using InControl;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {

    public GameObject m_Drone, m_Core, m_Shields, m_Bullet;
    public Color m_PlayerColor;
    public Transform m_BulletSpawn;
    public float m_BulletSpeed = 10f;
    public bool m_CanShoot = true;
    public int m_PlayerID = 0;

    public Renderer[] m_Renderer;




    // Public variables
    // Movement
    public float moveSpeed; // Acceleration speed
    public float maxSpeed; // Max movement speed
    public float tankRotSpeed = 0.02f; // Speed of rotation
    public float droneSpeed = 0.04f;
    // Private variable
    private float deadzone = 0.25f; // Analog stick deadzone
    private Rigidbody rb; // Player's rigidbody component
    private Vector3 movementInput; // Input for movement
    private Vector3 rotationInput; // Input for rotation
    private Vector3 rotation;
    private Vector3 tankRotation; // Current rotation
    private Vector3 turretRotation; // Current rotation
    private Vector3 spawn; // Player's starting position
    private Quaternion targetRotation;

    private float leftTrigger;
    private float rightTrigger;
    
    private string movementXName;
    private string movementYName;
    private string rotationXName;
    private string rotationYName;
    private string rightTriggerName;
    private string leftTriggerName;
    private string rightBumperName;
    private string leftBumperName;

    private float timer;
    public float fireDelay;
    public float randomSpreadX;
    public float projectileSpeed;
    

    public float exploForce = 100f;
    public float exploRadius = 5f;
    public float upwardsMod = 0.2f;

    private CameraScript m_CameraScript;

    private LMS m_LMS;

    private PlayerControllerManager m_ControllerManager;


    public InputDevice m_Device;
    private InputManager m_InputManager;
    
    public bool m_HasControls = false;
    private float m_Value = 360 / 8;

    private LineRenderer m_LineRenderer;

    public Material m_LineMaterial;

    void Awake()
    {
        m_ControllerManager = FindObjectOfType<PlayerControllerManager>();
        m_Device = m_ControllerManager.GiveInput(gameObject);
    }

    void Start()
    {
        m_LMS = FindObjectOfType<LMS>();
        rb = GetComponent<Rigidbody>(); // Setting rb as player's rigidbody component

        m_LineRenderer = gameObject.AddComponent<LineRenderer>();
        m_LineRenderer.material = m_LineMaterial;
        
        m_LineRenderer.SetWidth(0.2f, 0.2f);
        m_LineRenderer.SetVertexCount(2);
        m_LineRenderer.useWorldSpace = true;

        movementXName = "LSX" + m_PlayerID;
        movementYName = "LSY" + m_PlayerID;
        rotationXName = "RSX" + m_PlayerID;
        rotationYName = "RSY" + m_PlayerID;
        rightTriggerName = "RT" + m_PlayerID;
        leftTriggerName = "LT" + m_PlayerID;
        rightBumperName = "RB" + m_PlayerID;
        leftBumperName = "LB" + m_PlayerID;
        if(m_Device != null)
        {
            m_HasControls = true;
        }
        else
        {
            m_Device = m_ControllerManager.GiveInput(gameObject);
            m_HasControls = true;
        }
        m_LineRenderer.SetColors(m_PlayerColor, m_PlayerColor);
    }

    void Update()
    {
        timer += Time.deltaTime;
        HandleLaser();

    }

    void FixedUpdate()
    {
        if(m_LMS.m_RoundStarted)
        {
            Movement();
            Shooting();

        }
    }

    void Movement()
    {

        movementInput = new Vector3(m_Device.GetControl(InputControlType.LeftStickX).RawValue, 0, m_Device.GetControl(InputControlType.LeftStickY).RawValue);
        //rotationInput = new Vector3(Input.GetAxis(rotationXName), 0, Input.GetAxis(rotationYName));
        rotationInput = new Vector3(m_Device.GetControl(InputControlType.RightStickX), 0, m_Device.GetControl(InputControlType.RightStickY));

        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }
        if (rotationInput.magnitude < deadzone)
        {
            rotationInput = Vector3.zero;
        }
        // Rigidbody Movement
        if (movementInput.magnitude < deadzone)
        {
            movementInput = Vector3.zero;
        }
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movementInput * moveSpeed);
        }

        if (m_Device.GetControl(InputControlType.LeftBumper).IsPressed)
        {
            m_Core.transform.Rotate(0, -5, 0);
            m_Shields.transform.Rotate(0, -5, 0);
        }

        if (m_Device.GetControl(InputControlType.RightBumper).IsPressed)
        {
            m_Core.transform.Rotate(0, 5, 0);
            m_Shields.transform.Rotate(0, 5, 0);
        }

        if (rotationInput.x == 0f && rotationInput.z == 0f)
        { 
            //Checks if player is using right stick
        }

        Rotation();


    }

    void Shooting()
    {
        if (m_Device.GetControl(InputControlType.RightTrigger).IsPressed && timer > fireDelay && m_CanShoot)
        {
            timer = 0f;
            GameObject _bullet = (GameObject)Instantiate(m_Bullet, (new Vector3(m_BulletSpawn.position.x, m_BulletSpawn.position.y, m_BulletSpawn.position.z )), Quaternion.identity);
            _bullet.GetComponent<BulletScript>().m_ID = m_PlayerID;
            _bullet.GetComponent<Rigidbody>().AddForce(m_BulletSpawn.forward * m_BulletSpeed, ForceMode.Impulse);
            Destroy(_bullet, 3);
        }
    }

    void Rotation()
    {
        // Up = Between 337.5 & 22.5
        // Up right = Between 22.5 & 67.5
        // Right between 67.5 && 112.5
        // Right down between 112.5 && 157.5
        // Down between 157.5 & 202.5
        // Down left between 202.5 & 247.5
        // left between 247.5 & 292.5
        // Left up between 292.5 & 337.5
        if (rotationInput.magnitude > deadzone)
        {
            float _angle = m_Device.RightStick.Angle;
            if (_angle > 22.5 && _angle < 67.5) // Right Up
            {
                turretRotation.y = 45;
                // 45 
            }
            else if(_angle > 67.5 && _angle < 112.5) // Right
            {
                turretRotation.y = 90;
                // 90
            }
            else if(_angle > 112.5 && _angle < 157.5) // Right Down
            {
                turretRotation.y = 135;
                // 135
            }
            else if(_angle > 157.5 && _angle < 202.5) // Down
            {
                turretRotation.y = 180;
                // 180
            }
            else if(_angle > 202.5 && _angle < 247.5) // Left Down
            {
                turretRotation.y = 225;
                // 225
            }
            else if(_angle > 247.5 && _angle < 292.5) // Left
            {
                turretRotation.y = 270;
                // 270
            }
            else if(_angle > 292.5 && _angle < 337.5) // Left Up
            {
                turretRotation.y = 315;
                // 315
            }
            else
            {
                turretRotation.y = 0;
                // 0
            }





            //m_Drone.transform.rotation = Quaternion.LookRotation(rotationInput);
            m_Drone.transform.rotation = Quaternion.Lerp(Quaternion.Euler(turretRotation), m_Drone.transform.rotation, droneSpeed);
        }

    }

    void HandleLaser()
    {
        Ray _ray = new Ray(m_BulletSpawn.position, m_BulletSpawn.forward);
        RaycastHit _hit;
        m_LineRenderer.SetPosition(0, _ray.origin);
        if (Physics.Raycast(_ray, out _hit, 100f))
        {
            m_LineRenderer.SetPosition(1, _hit.point);
        }
        Debug.DrawLine(_ray.origin, _hit.point, Color.red);
    }
}
