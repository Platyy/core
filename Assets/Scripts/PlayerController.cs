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

    void Awake()
    {
        m_ControllerManager = FindObjectOfType<PlayerControllerManager>();
        m_Device = m_ControllerManager.GiveInput(gameObject);
    }

    void Start()
    {
        m_LMS = FindObjectOfType<LMS>();
        rb = GetComponent<Rigidbody>(); // Setting rb as player's rigidbody component

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
            gameObject.SetActive(false);
        }

    }

    void Update()
    {
        timer += Time.deltaTime;
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
        else if(rotationInput.magnitude > deadzone)
        {
            turretRotation = m_Drone.transform.rotation.eulerAngles;
            m_Drone.transform.rotation = Quaternion.LookRotation(rotationInput);
            m_Drone.transform.rotation = Quaternion.Lerp(Quaternion.Euler(turretRotation), m_Drone.transform.rotation, droneSpeed);
        }
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

}
